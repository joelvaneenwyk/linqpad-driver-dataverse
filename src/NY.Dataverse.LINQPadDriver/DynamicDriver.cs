#define TRACE

using LINQPad.Extensibility.DataContext;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.PowerPlatform.Dataverse.Client.Extensions;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Xml.Linq;
using System.Runtime.InteropServices;
using System.Globalization;
using System.IO;
using JetBrains.Annotations;

namespace NY.Dataverse.LINQPadDriver
{
    [PublicAPI]
    public class DynamicDriver : DynamicDataContextDriver
    {
        [UsedImplicitly]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "CodeQuality",
            "IDE0052:Remove unread private members",
            Justification = "<Pending>")]
        private static DynamicDriver? _driverInstance;

        private static ServiceClient? _dataverseServiceClient;
        private static QueryExecutionManager? _queryExecutionManager;

        static DynamicDriver()
        {
            AddDebuggerHook();
        }


        [Conditional("DEBUG")]
        private static void AddDebuggerHook()
        {
            // Uncomment the following code to attach to Visual Studio's debugger when an exception is thrown:
            AppDomain.CurrentDomain.FirstChanceException += (sender, args) =>
            {
                if (args.Exception.StackTrace?.Contains("NY.Dataverse.LINQPadDriver") ?? false)
                    Debugger.Launch();
            };
        }

        [Conditional("DEBUG")]
        private static void LaunchDebugger()
        {
            Debugger.Launch();
        }

        public override string Name => "Dataverse LINQPad Driver";


        public override string Author => "Natraj Yegnaraman";

        public override string GetConnectionDescription(IConnectionInfo cxInfo)
        {
            var connectionProperties = new ConnectionProperties(cxInfo);
            return !string.IsNullOrEmpty(connectionProperties.ConnectionName) ? $"{connectionProperties.ConnectionName} ({connectionProperties.EnvironmentUrl})" : connectionProperties.EnvironmentUrl;
        }

        public override bool ShowConnectionDialog(IConnectionInfo cxInfo, ConnectionDialogOptions dialogOptions)
            => new ConnectionDialog(cxInfo).ShowDialog() == true;

        [SuppressMessage("Style", "IDE0300:Collection initialization can be simplified",
            Justification = "Not supported on older .NET versions.")]
        public override IReadOnlyList<string> GetAssembliesToAdd(IConnectionInfo cxInfo)
        {
            return new[]
                {
                    //"Azure.Identity.dll",
                    "Microsoft.PowerPlatform.Dataverse.Client.dll",
                    "Microsoft.Xrm.Sdk.dll",
                    "Microsoft.Crm.Sdk.Proxy.dll"
                };
        }

        [Conditional("DEBUG")]
        private void SaveContent(string code) =>
            File.WriteAllText(Path.Combine(GetContentFolder(), "LINQPad.EarlyBound.cs"), code);

        [PublicAPI]
        public static string TransformText(
            EntityMetadataCollection? metadata, string? ns, string? typeName)
        {
            var template = new CDSTemplate(metadata, ns, typeName);
            MethodInfo? transformText = typeof(CDSTemplate).GetMethod("TransformText");
            var result = transformText?.Invoke(template, null);
            return result?.ToString() ?? string.Empty;
        }

        [SuppressMessage(
            "Style",
            "IDE0028:Collection initialization can be simplified",
            Justification = "Not supported on older .NET versions.")]
        public override List<ExplorerItem> GetSchemaAndBuildAssembly(
            IConnectionInfo cxInfo, AssemblyName assemblyToBuild, ref string nameSpace, ref string typeName)
        {
            nameSpace = "NY.Dataverse.LINQPadDriver";
            typeName = "LINQPadOrganizationServiceContext";

            LaunchDebugger();

            var connectionProperties = new ConnectionProperties(cxInfo);
            List<ExplorerItem> explorerItems = new();
            ServiceClient? client = null;
            try
            {
                client = connectionProperties.GetCdsClient();
                if (client.IsReady)
                {
                    var entityMetadata = GetEntityMetadata(client);
                    if (entityMetadata != null)
                    {
                        string code = TransformText(entityMetadata, nameSpace, typeName);

                        SaveContent(code);

                        Compile(code, assemblyToBuild.CodeBase, cxInfo);

                        BuildEntityAndAttributeExplorerItems(explorerItems, entityMetadata);

                        foreach (ref var entity in CollectionsMarshal.AsSpan(entityMetadata))
                        {
                            var entityLogicalName = entity.entityMetadata.LogicalName;
                            var source = explorerItems.FirstOrDefault(
                                e => e.Kind == ExplorerItemKind.QueryableObject
                                && (string)e.Tag == entityLogicalName);

                            BuildOneToManyRelationLinks(explorerItems, entity, source);
                            BuildManyToOneRelationLinks(explorerItems, entity, source);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error occurred while attemoting to connect to {connectionProperties.EnvironmentUrl} using {connectionProperties.AuthenticationType}: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (client != null)
                {
                    var user = client.Retrieve("systemuser", client.GetMyUserId(), new ColumnSet("fullname", "internalemailaddress"));
                    var email = user.GetAttributeValue<string>("internalemailaddress");
                    var userName = user.GetAttributeValue<string>("fullname");
                    MessageBox.Show($"Connected to {client.OrganizationDetail.FriendlyName} as {userName} ({email})", "Connected", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            return explorerItems;
        }

        public override void InitializeContext(IConnectionInfo cxInfo, object context,
                                                QueryExecutionManager executionManager)
        {
            _driverInstance = this;
            var preExecuteEvent = context.GetType().GetEvent("PreExecute");
            var preExecuteEventHandler = GetType().GetMethod("OnPreExecute", BindingFlags.Static | BindingFlags.NonPublic);
            if (preExecuteEvent?.EventHandlerType != null && preExecuteEventHandler != null)
            {
                preExecuteEvent?.AddEventHandler(context, Delegate.CreateDelegate(preExecuteEvent.EventHandlerType, null, preExecuteEventHandler));
            }
            _queryExecutionManager = executionManager;
            base.InitializeContext(cxInfo, context, executionManager);
        }

        public override bool AreRepositoriesEquivalent(IConnectionInfo c1, IConnectionInfo c2)
        {
            return Equals(c1.DriverData.Element("EnvironmentUrl"), c2.DriverData.Element("EnvironmentUrl"));
        }

        public override object[] GetContextConstructorArguments(IConnectionInfo cxInfo)
        {
            var connectionProperties = new ConnectionProperties(cxInfo);
            var dataverseServiceClient = _dataverseServiceClient ?? connectionProperties.GetCdsClient();
            if (_dataverseServiceClient == null || !_dataverseServiceClient.Equals(dataverseServiceClient))
            {
                _dataverseServiceClient = dataverseServiceClient;
            }
            return new[]
            {
                dataverseServiceClient
            };
        }

        [SuppressMessage("Style", "IDE0300:Collection initialization can be simplified",
            Justification = "Not supported on older .NET versions.")]
        public override ParameterDescriptor[] GetContextConstructorParameters(IConnectionInfo cxInfo) =>
            new[]
        {
            new ParameterDescriptor("dataverseServiceClient", typeof(ServiceClient).FullName)
        };

        [SuppressMessage("Style", "IDE0028:Collection initialization can be simplified",
            Justification = "Not supported on older .NET versions.")]
        public override IReadOnlyList<string> GetNamespacesToAdd(IConnectionInfo cxInfo) =>
            new List<string>
        {
            "Microsoft.Crm.Sdk.Messages",
            "Microsoft.Xrm.Sdk",
            "Microsoft.Xrm.Sdk.Query",
            "Microsoft.Xrm.Sdk.Client",
            "Microsoft.Xrm.Sdk.Messages",
            "Microsoft.Crm.Sdk.Messages",
            "Microsoft.Xrm.Sdk.Metadata",
            "Microsoft.Xrm.Sdk.Discovery",
            "Microsoft.Xrm.Sdk.Extensions",
            "Microsoft.Xrm.Sdk.Linq",
            "Microsoft.Xrm.Sdk.WebServiceClient",
            "Microsoft.PowerPlatform.Dataverse.Client",
            "Microsoft.PowerPlatform.Dataverse.Client.Extensions",
            "NY.Dataverse.LINQPadDriver.Entities"
        };

        [SuppressMessage("Style", "IDE0300:Collection initialization can be simplified",
            Justification = "Not supported on older .NET versions.")]
        static void Compile(string cSharpSourceCode, string? outputFile, IConnectionInfo cxInfo)
        {
            var customAssemblies = new[]{
                typeof(ServiceClient).Assembly.Location,
                typeof(EntityReference).Assembly.Location,
                typeof(AddAppComponentsRequest).Assembly.Location
            };
            var assembliesToReference = GetCoreFxReferenceAssemblies(cxInfo).Concat(customAssemblies);
            // CompileSource is a static helper method to compile C# source code using LINQPad's built-in Roslyn libraries.
            // If you prefer, you can add a NuGet reference to the Roslyn libraries and use them directly.
            var compileResult = CompileSource(new CompilationInput
            {
                FilePathsToReference = assembliesToReference.ToArray(),
                OutputPath = outputFile,
                SourceCode = new[] { cSharpSourceCode }
            });
            if (compileResult.Errors.Length > 0)
                throw new TypeCompileException($"Cannot compile typed context: {compileResult.Errors[0]}");
        }

        [Serializable]
        public class TypeCompileException : Exception
        {
            public TypeCompileException() : base() { }
            public TypeCompileException(string message) : base(message) { }
            public TypeCompileException(string message, Exception inner) : base(message, inner) { }
        }

        #region Helper Methods
        private static void OnPreExecute(object sender, EventArgs e)
        {
            if (_dataverseServiceClient != null
                && e.GetType()?.GetProperty("query")?.GetValue(e) is QueryExpression query)
            {
                var expressionToFetchXmlRequest = new QueryExpressionToFetchXmlRequest
                {
                    Query = query
                };
                var organizationResponse = (QueryExpressionToFetchXmlResponse)_dataverseServiceClient.Execute(expressionToFetchXmlRequest);
                try
                {
                    var webApiUrl = WebAPIQueryHelper.GetWebApiUrl(_dataverseServiceClient, organizationResponse.FetchXml);
                    if (!string.IsNullOrEmpty(webApiUrl))
                    {
                        _queryExecutionManager?.SqlTranslationWriter.WriteLine($"***WebAPI Url***\n{webApiUrl}");
                    }
                }
                catch (Exception ex)
                {
                    _queryExecutionManager?.SqlTranslationWriter.WriteLine($"***WebAPI Generation Exception***\n{ex.Message}");
                }
                finally
                {
                    _queryExecutionManager?.SqlTranslationWriter.WriteLine($"\n***FetchXML***\n{XElement.Parse(organizationResponse.FetchXml)}\n");
                }
            }
        }

        private static EntityMetadataCollection? GetEntityMetadata(ServiceClient client)
        {
            var metadata = client.GetAllEntityMetadata(filter: EntityFilters.Attributes | EntityFilters.Entity | EntityFilters.Relationships).ToList();
            //Fix for https://github.com/rajyraman/Dataverse-LINQPad-Driver/issues/19
            metadata.ForEach(entityMetadata =>
            {
                entityMetadata.SchemaName = !IsCSharpKeyword(entityMetadata.SchemaName) ? entityMetadata.SchemaName : $"_{entityMetadata.SchemaName}";
                entityMetadata.EntitySetName = entityMetadata.SchemaName;
            });
            var result = (from e in metadata
                          orderby e.LogicalName
                          select (entityMetadata: e, optionMetadata: (
                                      from attribute in e.Attributes
                                          .Where(a =>
                                              a.AttributeType == AttributeTypeCode.State
                                              || a.AttributeType == AttributeTypeCode.Status
                                              || a.AttributeType == AttributeTypeCode.Picklist)
                                          .OrderBy(a => a.LogicalName)
                                      let allOptions = from a in (
                                              (EnumAttributeMetadata)attribute).OptionSet.Options
                                                       select new
                                                       {
                                                           a.Label,
                                                           a.Value,
                                                           SanitisedLabel = a.Label.UserLocalizedLabel?.Label.Sanitise() ?? string.Empty
                                                       }
                                      select (
                                          attributeName: attribute.SchemaName,
                                          options: allOptions
                                              .Select(x =>
                                              {
                                                  var enumValue = x.SanitisedLabel;
                                                  if (string.IsNullOrEmpty(x.SanitisedLabel))
                                                  {
                                                      //When the value is a negative number, replace '-' with '_'.
                                                      enumValue = $"_{x.Value}".Replace("-", "_");
                                                  }
                                                  else if (IsCSharpKeyword(enumValue)
                                                           || char.IsDigit(enumValue[0])
                                                           || allOptions.Count(o =>
                                                               o.SanitisedLabel == x.SanitisedLabel) > 1)
                                                  {
                                                      //When the value is a negative number, replace '-' with '_'.
                                                      enumValue = $"_{enumValue}_{x.Value}".Replace("-", "_");
                                                  }
                                                  return (Label: enumValue, x.Value);
                                              }).ToList()
                                          )
                                      ).ToList()
                          )).ToList();

            return EntityMetadataCollection.TryCreate(result);
        }

        private static void BuildEntityAndAttributeExplorerItems(List<ExplorerItem> explorerItems, List<(EntityMetadata entityMetadata, List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata)> entityMetadata)
        {
            foreach (ref var entity in CollectionsMarshal.AsSpan(entityMetadata))
            {
                var attributes = entity.entityMetadata.Attributes
                .Where(x => (x.IsLogical == false || (x.IsLogical == true && x.IsValidForForm == true))
                && x.AttributeType != AttributeTypeCode.Virtual
                && x.AttributeType != AttributeTypeCode.CalendarRules)
                .OrderBy(x => x.LogicalName)
                .Select(a =>
                {
                    var attributeName = a.SchemaName;
                    if (a.LogicalName == a.EntityLogicalName ||
                        a.SchemaName == "EntityLogicalName" ||
                        a.SchemaName == "EntityTypeCode" ||
                        a.SchemaName == "Id")
                    {
                        attributeName = $"{a.SchemaName}1";
                    }
                    if (a.AttributeType == AttributeTypeCode.PartyList)
                    {
                        attributeName = char.ToUpper(attributeName[0], CultureInfo.CurrentCulture) + attributeName[1..];
                    }
                    return new ExplorerItem($"{attributeName} ({GetTypeFromCode(a.AttributeType)})", ExplorerItemKind.Parameter, ExplorerIcon.Column)
                    {
                        Icon = a.IsPrimaryId == true ? ExplorerIcon.Key : ExplorerIcon.Column,
                        Tag = a.LogicalName
                    };
                }).ToList();
                ExplorerItem item = new(entity.entityMetadata.SchemaName, ExplorerItemKind.QueryableObject, ExplorerIcon.Table)
                {
                    IsEnumerable = true,
                    Children = attributes,
                    Tag = entity.entityMetadata.LogicalName
                };
                explorerItems.Add(item);
            }
        }

        private static void BuildOneToManyRelationLinks(List<ExplorerItem> explorerItems, (EntityMetadata entityMetadata, List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata) entity, ExplorerItem? source)
        {
            foreach (var oneToMany in entity.entityMetadata.OneToManyRelationships)
            {
                var target = explorerItems.FirstOrDefault(e => e.Kind == ExplorerItemKind.QueryableObject && (string)e.Tag == oneToMany.ReferencingEntity);
                if (target != null)
                {
                    source?.Children.Add(new ExplorerItem(oneToMany.SchemaName, ExplorerItemKind.CollectionLink, ExplorerIcon.OneToMany)
                    {
                        HyperlinkTarget = target,
                        ToolTipText = oneToMany.ReferencingAttribute
                    });
                }
            }
        }

        private static void BuildManyToOneRelationLinks(List<ExplorerItem> explorerItems, (EntityMetadata entityMetadata, List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata) entity, ExplorerItem? source)
        {
            foreach (var manyToOne in entity.entityMetadata.ManyToOneRelationships)
            {
                var targetEntity = explorerItems.FirstOrDefault(e => e.Kind == ExplorerItemKind.QueryableObject && (string)e.Tag == manyToOne.ReferencedEntity);
                var targetAttribute = targetEntity?.Children.FirstOrDefault(e => e.Kind == ExplorerItemKind.Parameter && (string)e.Tag == manyToOne.ReferencedAttribute);
                if (targetAttribute != null)
                {
                    source?.Children.Add(new ExplorerItem(manyToOne.SchemaName, ExplorerItemKind.ReferenceLink, ExplorerIcon.ManyToOne)
                    {
                        HyperlinkTarget = targetAttribute,
                        ToolTipText = manyToOne.ReferencingAttribute
                    });
                }
            }
        }

        private static string GetTypeFromCode(AttributeTypeCode? attributeTypeCode)
        {
            var attributeType = "object";
            switch (attributeTypeCode)
            {
                case AttributeTypeCode.Integer:
                    attributeType = "Whole Number";
                    break;
                case AttributeTypeCode.BigInt:
                    attributeType = "Big Integer";
                    break;
                case AttributeTypeCode.Boolean:
                case AttributeTypeCode.ManagedProperty:
                    attributeType = "Yes/No";
                    break;
                case AttributeTypeCode.Lookup:
                    attributeType = "Lookup";
                    break;
                case AttributeTypeCode.Customer:
                    attributeType = "Customer";
                    break;
                case AttributeTypeCode.Owner:
                    attributeType = "Owner";
                    break;
                case AttributeTypeCode.DateTime:
                    attributeType = "DateTime";
                    break;
                case AttributeTypeCode.Decimal:
                    attributeType = "Decimal";
                    break;
                case AttributeTypeCode.Double:
                    attributeType = "Double";
                    break;
                case AttributeTypeCode.String:
                    attributeType = "Text";
                    break;
                case AttributeTypeCode.EntityName:
                    attributeType = "Entity Name";
                    break;
                case AttributeTypeCode.Memo:
                    attributeType = "Multiline Text";
                    break;
                case AttributeTypeCode.Money:
                    attributeType = "Currency";
                    break;
                case AttributeTypeCode.Picklist:
                case AttributeTypeCode.State:
                case AttributeTypeCode.Status:
                    attributeType = "Choices";
                    break;
                case AttributeTypeCode.Uniqueidentifier:
                    attributeType = "Unique Identifier";
                    break;
                case AttributeTypeCode.PartyList:
                    attributeType = "Party List";
                    break;
            }
            return attributeType;
        }

        #endregion
    }
}
