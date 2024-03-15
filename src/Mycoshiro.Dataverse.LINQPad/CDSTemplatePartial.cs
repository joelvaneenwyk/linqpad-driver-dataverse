using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Mycoshiro.Dataverse.LINQPad
{
    using EntityMetadataList = List<(EntityMetadata entityMetadata, List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata)>;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class CDSTemplate
    {
        [PublicAPI]
        public string? Namespace { get; init; }

        [PublicAPI]
        public string TypeName { get; set; }

        [PublicAPI]
        public EntityMetadataList Metadata { get; private set; }

        [SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
        [SuppressMessage("Style", "IDE0290:Use primary constructor",
            Justification = "Not supported on older .NET versions.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression",
            Justification = "It is not unnecessary but rather ReSharper only.")]
        [SuppressMessage("ReSharper", "UseCollectionExpression")]
        public CDSTemplate(
            EntityMetadataList? metadata = null,
            string? ns = null,
            string? typeName = null)
        {
#pragma warning disable IDE0028
            Metadata = metadata ?? new EntityMetadataList();
#pragma warning restore IDE0028

            TypeName = typeName ?? "InvalidType";
            Namespace = ns ?? "InvalidNamespace";
        }

        [PublicAPI]
        public static string TransformText(
            EntityMetadataList? metadata,
            string? ns,
            string? typeName)
        {
            var template = new CDSTemplate(metadata, ns, typeName);
            return template.TransformText();
        }
    }
}
