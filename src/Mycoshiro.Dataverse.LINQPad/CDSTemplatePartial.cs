using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Mycoshiro.Dataverse.LINQPad
{
    using EntityMetadataList = List<(EntityMetadata entityMetadata, List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata)>;

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    partial class CDSTemplate
    {
        public string Namespace { get; init; }

        public string TypeName { get; set; }

        [PublicAPI]
        public EntityMetadataList Metadata { get; private set; }

        public CDSTemplate(
            EntityMetadataList? metadata = null,
            string? ns = null,
            string? typeName = null)
        {
            Metadata = metadata ?? new();
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
