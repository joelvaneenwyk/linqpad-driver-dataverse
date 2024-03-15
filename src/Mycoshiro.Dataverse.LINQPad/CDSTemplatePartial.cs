using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Mycoshiro.Dataverse.LINQPad
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    partial class CDSTemplate
    {
        public string Namespace { get; init; }

        public string TypeName { get; set; }

        [PublicAPI]
        public List<(EntityMetadata entityMetadata, List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata)> Metadata { get; private set; }

        public CDSTemplate(
            List<(
                EntityMetadata entityMetadata,
                List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata
                )>? metadata = null,
            string? ns = null,
            string? typeName = null)
        {
            Metadata = metadata ?? new();
            TypeName = typeName ?? "InvalidType";
            Namespace = ns ?? "InvalidNamespace";
        }

        [PublicAPI]
        public static string TransformText(
            List<(
                EntityMetadata entityMetadata,
                List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata
                )>? metadata,
            string? ns,
            string? typeName)
        {
            var template = new CDSTemplate(metadata, ns, typeName);
            return template.TransformText();
        }
    }
}
