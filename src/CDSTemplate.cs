﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace NY.Dataverse.LINQPadDriver
{
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using Microsoft.Xrm.Sdk;
    using Microsoft.Xrm.Sdk.Metadata;
    using System;

    /// <summary>
    /// Class to produce the template output
    /// </summary>

    #line 1 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class CDSTemplate : CDSTemplateBase
    {
#line hidden
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write(@"
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Xml.Linq;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Metadata;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Linq;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;

[assembly: Microsoft.Xrm.Sdk.Client.ProxyTypesAssemblyAttribute()]
namespace ");

            #line 28 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Namespace));

            #line default
            #line hidden
            this.Write(".Entities\r\n{\r\n\t");

            #line 30 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
foreach (var entity in Metadata)
	{

            #line default
            #line hidden
            this.Write("\r\n\t\t[System.Runtime.Serialization.DataContractAttribute()]\r\n\t\t[Microsoft.Xrm.Sdk." +
                    "Client.EntityLogicalNameAttribute(\"");

            #line 34 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.LogicalName));

            #line default
            #line hidden
            this.Write("\")]\r\n\t\tpublic partial class ");

            #line 35 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.SchemaName));

            #line default
            #line hidden
            this.Write("Table : Microsoft.Xrm.Sdk.Entity, System.ComponentModel.INotifyPropertyChanging, " +
                    "System.ComponentModel.INotifyPropertyChanged\r\n\t\t{\r\n\t\r\n\t\t\tpublic ");

            #line 38 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.SchemaName));

            #line default
            #line hidden
            this.Write("Table() : \r\n\t\t\t\t\tbase(\"");

            #line 39 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.LogicalName));

            #line default
            #line hidden
            this.Write("\")\r\n\t\t\t{\r\n\t\t\t}\r\n\t\r\n\t\t\tpublic const string EntityLogicalName = \"");

            #line 43 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.LogicalName));

            #line default
            #line hidden
            this.Write("\";\r\n\t\t\t\r\n\t\t\tpublic const string EntitySchemaName = \"");

            #line 45 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.SchemaName));

            #line default
            #line hidden
            this.Write("\";\r\n\t\t\r\n\t\t\tpublic const string PrimaryIdAttribute = \"");

            #line 47 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.PrimaryIdAttribute));

            #line default
            #line hidden
            this.Write("\";\r\n\t\t\r\n\t\t\tpublic const string PrimaryNameAttribute = \"");

            #line 49 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((entity.entityMetadata.PrimaryNameAttribute ?? "")));

            #line default
            #line hidden
            this.Write("\";\r\n\t\t\t\r\n\t\t\tpublic const string EntityLogicalCollectionName = \"");

            #line 51 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture((entity.entityMetadata.LogicalCollectionName ?? "")));

            #line default
            #line hidden
            this.Write("\";\r\n\t\r\n\t\t\tpublic const int EntityTypeCode = ");

            #line 53 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.ObjectTypeCode));

            #line default
            #line hidden
            this.Write(@";

			public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

			public event System.ComponentModel.PropertyChangingEventHandler PropertyChanging;

			private void OnPropertyChanged(string propertyName)
			{
				if ((this.PropertyChanged != null))
				{
					this.PropertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
				}
			}

			private void OnPropertyChanging(string propertyName)
			{
				if ((this.PropertyChanging != null))
				{
					this.PropertyChanging(this, new System.ComponentModel.PropertyChangingEventArgs(propertyName));
				}
			}
			");

            #line 74 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
foreach (var attribute in entity.entityMetadata.Attributes.Where(x=>(x.IsLogical == false
			|| (x.IsLogical == true && x.IsValidForForm == true)) && x.AttributeType != AttributeTypeCode.Virtual
			&& x.AttributeType != AttributeTypeCode.CalendarRules))
			{
				var attributeType = GetTypeFromCode(attribute.AttributeType);
				var attributeName = (attribute.SchemaName == entity.entityMetadata.SchemaName ||
				attribute.SchemaName == "EntityLogicalName" ||
				attribute.SchemaName == "EntityTypeCode" ||
				attribute.SchemaName == "Id")
				? $"{attribute.SchemaName}1" : attribute.SchemaName;


            #line default
            #line hidden
            this.Write("\t\t\t\t");

            #line 85 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
if(attribute.LogicalName == entity.entityMetadata.PrimaryIdAttribute)
				{

            #line default
            #line hidden
            this.Write("\t\t\t\t\t[AttributeLogicalNameAttribute(\"");

            #line 87 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\")]\r\n\t\t\t\t\tpublic Guid? ");

            #line 88 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\tget\r\n\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\treturn this.GetAttributeValue<Guid?>(\"");

            #line 92 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t\t}\r\n\t\t\t\t\t\tset\r\n\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\tthis.OnPropertyChanging(\"");

            #line 96 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t\t\tthis.SetAttributeValue(\"");

            #line 97 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\", value);\r\n\t\t\t\t\t\t\tif (value.HasValue)\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tbase.Id = value.Value;\r" +
                    "\n\t\t\t\t\t\t\t}\r\n\t\t\t\t\t\t\telse\r\n\t\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\t\tbase.Id = System.Guid.Empty;\r\n\t\t\t\t\t\t\t}" +
                    "\r\n\t\t\t\t\t\t\tthis.OnPropertyChanged(\"");

            #line 106 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t\t}\r\n\t\t\t\t\t}\r\n\t\t\t\t\t[AttributeLogicalNameAttribute(\"");

            #line 109 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\")]\r\n\t\t\t\t\tpublic override Guid Id\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\tget\r\n\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\treturn this" +
                    ".GetAttributeValue<Guid>(\"");

            #line 114 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t\t}\r\n\t\t\t\t\t\tset\r\n\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\tthis.");

            #line 118 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write(" = value;\r\n\t\t\t\t\t\t}\r\n\t\t\t\t\t}\r\n\t\t\t\t");

            #line 121 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
} else if(attributeType == "PartyList")
				{
					attributeName = char.ToUpper(attributeName[0]) + attributeName[1..];


            #line default
            #line hidden
            this.Write("\t\t\t\t[AttributeLogicalNameAttribute(\"");

            #line 125 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\")]\r\n\t\t\t\tpublic IEnumerable<ActivityPartyTable> ");

            #line 126 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t{\r\n\t\t\t\t\tget\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\tvar collection = this.GetAttributeValue<EntityCo" +
                    "llection>(\"");

            #line 130 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write(@""");
						if (collection != null && collection.Entities != null)
						{
							return Enumerable.Cast<ActivityPartyTable>(collection.Entities);
						}
						else
						{
							return null;
						}
					}
					set
					{
						this.OnPropertyChanging(""");

            #line 142 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t\tif ((value == null))\r\n\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\tthis.SetAttributeValue(\"");

            #line 145 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\", value);\r\n\t\t\t\t\t\t}\r\n\t\t\t\t\t\telse\r\n\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\tthis.SetAttributeValue(\"");

            #line 149 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\", \r\n\t\t\t\t\t\t\tnew EntityCollection(new List<Microsoft.Xrm.Sdk.Entity>(value)));\r\n\t\t" +
                    "\t\t\t\t}\r\n\t\t\t\t\t\tthis.OnPropertyChanged(\"");

            #line 152 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t}\r\n\t\t\t\t}\r\n\t\t\t\t");

            #line 155 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
} else if(attributeType == "OptionSetValue")
				{
				   var enumName = $"{entity.entityMetadata.SchemaName}_{attribute.SchemaName}";


            #line default
            #line hidden
            this.Write("\t\t\t\t\t[AttributeLogicalNameAttribute(\"");

            #line 159 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\")]\r\n\t\t\t\t\tpublic virtual ");

            #line 160 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(enumName));

            #line default
            #line hidden
            this.Write("? ");

            #line 160 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\t[System.Diagnostics.DebuggerNonUserCode()]\r\n\t\t\t\t\t\tget\r\n\t\t\t\t\t\t{\r\n\t" +
                    "\t\t\t\t\t\treturn (");

            #line 165 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(enumName));

            #line default
            #line hidden
            this.Write("?)this.GetAttributeValue<OptionSetValue>(\"");

            #line 165 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\")?.Value;\r\n\t\t\t\t\t\t}\r\n\t\t\t\t");

            #line 167 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
if(attribute.IsValidForUpdate == true || attribute.IsValidForCreate == true)
				{

            #line default
            #line hidden
            this.Write("\t\t\t\t\t\t[System.Diagnostics.DebuggerNonUserCode()]\r\n\t\t\t\t\t\tset\r\n\t\t\t\t\t\t{\r\n\t\t\t\t\t\t\tthis" +
                    ".OnPropertyChanging(\"");

            #line 172 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t\t\tthis.SetAttributeValue(\"");

            #line 173 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\", value.HasValue ? new OptionSetValue((int)value) : null);\r\n\t\t\t\t\t\t\tthis.OnProper" +
                    "tyChanged(\"");

            #line 174 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t\t}\r\n\t\t\t\t");

            #line 176 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write("\t\t\t\t\t}\r\n\t\t\t\t");

            #line 178 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
} else {

            #line default
            #line hidden
            this.Write("\t\t\t\t[AttributeLogicalNameAttribute(\"");

            #line 179 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\")]\t\t\r\n\t\t\t\tpublic ");

            #line 180 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeType));

            #line default
            #line hidden
            this.Write(" ");

            #line 180 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\r\n\t\t\t\t{\r\n\t\t\t\t\tget\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\treturn this.GetAttributeValue<");

            #line 184 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeType));

            #line default
            #line hidden
            this.Write(">(\"");

            #line 184 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\")");

            #line 184 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.AttributeType == AttributeTypeCode.DateTime ? "?.ToLocalTime()" : ""));

            #line default
            #line hidden
            this.Write(";\r\n\t\t\t\t\t}\r\n\t\t\t\t");

            #line 186 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
if(attribute.IsValidForUpdate == true || attribute.IsValidForCreate == true)
				{

            #line default
            #line hidden
            this.Write("\t\t\t\t\tset\r\n\t\t\t\t\t{\r\n\t\t\t\t\t\tthis.OnPropertyChanging(\"");

            #line 190 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t\tthis.SetAttributeValue(\"");

            #line 191 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attribute.LogicalName));

            #line default
            #line hidden
            this.Write("\", value);\r\n\t\t\t\t\t\tthis.OnPropertyChanged(\"");

            #line 192 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(attributeName));

            #line default
            #line hidden
            this.Write("\");\r\n\t\t\t\t\t}\r\n\t\t\t\t");

            #line 194 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write("\t\t\t\t}\r\n\t\t\t\t");

            #line 196 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write("\t\t\t");

            #line 197 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write("\t\t}\r\n\t");

            #line 199 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write("}\r\n\r\nnamespace ");

            #line 202 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Namespace));

            #line default
            #line hidden
            this.Write("\r\n{\r\n\tusing Entities;\r\n\tpublic static class Tables\r\n\t{\r\n\t");

            #line 207 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
foreach (var entity in Metadata)
	{

            #line default
            #line hidden
            this.Write("\t\tpublic static string ");

            #line 209 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.SchemaName));

            #line default
            #line hidden
            this.Write(" = \"");

            #line 209 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.LogicalName));

            #line default
            #line hidden
            this.Write("\";\r\n\t");

            #line 210 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write("\t}\r\n\t");

            #line 212 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
foreach (var entity in Metadata)
	{

            #line default
            #line hidden
            this.Write("\t\t");

            #line 214 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
foreach (var optionMetadata in entity.optionMetadata)
		{

            #line default
            #line hidden
            this.Write("\t\t\t[System.Runtime.Serialization.DataContractAttribute()]\r\n\t\t\tpublic enum ");

            #line 217 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.SchemaName));

            #line default
            #line hidden
            this.Write("_");

            #line 217 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(optionMetadata.attributeName));

            #line default
            #line hidden
            this.Write("\r\n\t\t\t{\r\n\t\t\t\t");

            #line 219 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
foreach (var option in optionMetadata.options)
				{


            #line default
            #line hidden
            this.Write("\t\t\t\t\t[System.Runtime.Serialization.EnumMemberAttribute()]\r\n\t\t\t\t\t");

            #line 223 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(option.Label));

            #line default
            #line hidden
            this.Write(" = ");

            #line 223 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(option.Value));

            #line default
            #line hidden
            this.Write(",\r\n\t\t\t\t");

            #line 224 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write("\t\t\t}\t\t\t\r\n\t\t");

            #line 226 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write("\t");

            #line 227 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write(@"
	public class LINQPadOrganizationServiceContext : OrganizationServiceContext
	{
        public event EventHandler PreExecute;

        public LINQPadOrganizationServiceContext(IOrganizationService service) : base(service)
        {
            this.DataverseClient = (ServiceClient)service;
        }

        protected override void OnExecute(OrganizationRequest request, OrganizationResponse response)
        {
			if(PreExecute != null && request is RetrieveMultipleRequest r)
			{
                PreExecute(this, new OnExecuteEventArgs((QueryExpression)r.Query));
            }
            base.OnExecute(request, response);
        }

        public ServiceClient DataverseClient
        {
            get;
            private set;
        }

		");

            #line 253 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
 foreach (var entity in Metadata)
		{


            #line default
            #line hidden
            this.Write("\t\tpublic IQueryable<");

            #line 256 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Namespace));

            #line default
            #line hidden
            this.Write(".Entities.");

            #line 256 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.SchemaName));

            #line default
            #line hidden
            this.Write("Table> ");

            #line 256 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.EntitySetName));

            #line default
            #line hidden
            this.Write("\r\n\t\t{\r\n\t\t\t[System.Diagnostics.DebuggerNonUserCode()]\r\n\t\t\tget\r\n\t\t\t{\r\n\t\t\t\treturn th" +
                    "is.CreateQuery<");

            #line 261 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(this.Namespace));

            #line default
            #line hidden
            this.Write(".Entities.");

            #line 261 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
            this.Write(this.ToStringHelper.ToStringWithCulture(entity.entityMetadata.SchemaName));

            #line default
            #line hidden
            this.Write("Table>();\r\n\t\t\t}\r\n\t\t}\r\n\t\t");

            #line 264 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"
}

            #line default
            #line hidden
            this.Write("\t}\r\n\r\n\tpublic class OnExecuteEventArgs : EventArgs\r\n\t{\r\n\t\tpublic QueryExpression " +
                    "query { get; set; }\r\n\t\tpublic OnExecuteEventArgs(QueryExpression query)\r\n\t\t{\r\n\t\t" +
                    "\tthis.query = query;\r\n\t\t}\r\n\t}\r\n}\r\n\r\n");
            return this.GenerationEnvironment.ToString();
        }

        #line 277 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"

public string Namespace { get; set; }

public string TypeName { get; set; }


        #line default
        #line hidden

        #line 283 "D:\GitHub\Dataverse-LINQPad-Driver\src\CDSTemplate.tt"

string GetTypeFromCode(AttributeTypeCode? attributeTypeCode)
{
	var attributeType = "object";
	switch (attributeTypeCode)
	{
		case AttributeTypeCode.BigInt:
		case AttributeTypeCode.Integer:
			attributeType = "int";
			break;
		case AttributeTypeCode.Boolean:
			attributeType = "bool";
			break;
		case AttributeTypeCode.ManagedProperty:
			attributeType = "BooleanManagedProperty";
			break;
		case AttributeTypeCode.Customer:
		case AttributeTypeCode.Lookup:
		case AttributeTypeCode.Owner:
			attributeType = "EntityReference";
			break;
		case AttributeTypeCode.DateTime:
			attributeType = "DateTime?";
			break;
		case AttributeTypeCode.Decimal:
			attributeType = "decimal";
			break;
		case AttributeTypeCode.Double:
			attributeType = "double";
			break;
		case AttributeTypeCode.EntityName:
		case AttributeTypeCode.Memo:
		case AttributeTypeCode.String:
			attributeType = "string";
			break;
		case AttributeTypeCode.Money:
			attributeType = "Money";
			break;
		case AttributeTypeCode.Picklist:
		case AttributeTypeCode.State:
		case AttributeTypeCode.Status:
			attributeType = "OptionSetValue";
			break;
		case AttributeTypeCode.Uniqueidentifier:
			attributeType = "Guid";
			break;
		case AttributeTypeCode.PartyList:
			attributeType = "PartyList";
			break;
	}
	return attributeType;
}


        #line default
        #line hidden
    }

    #line default
    #line hidden
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class CDSTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        public System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0)
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
