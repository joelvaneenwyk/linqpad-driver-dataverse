using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Mycoshiro.Dataverse.LINQPad
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    partial class CDSTemplate
    {
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
        public List<(EntityMetadata entityMetadata, List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata)> Metadata { get; private set; }
    }
}
