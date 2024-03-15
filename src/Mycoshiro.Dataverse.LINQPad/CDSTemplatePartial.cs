using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace Mycoshiro.Dataverse.LINQPad
{
    using EntityMetadataList = List<(
        EntityMetadata entityMetadata,
        List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata)>;

    [PublicAPI]
    public sealed class EntityMetadataCollection : EntityMetadataList
    {
#pragma warning disable IDE0028
        [SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
        [SuppressMessage("Style", "IDE0290:Use primary constructor",
            Justification = "Not supported on older .NET versions.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression",
            Justification = "It is not unnecessary but rather ReSharper only.")]
        [SuppressMessage("ReSharper", "UseCollectionExpression")]
        internal EntityMetadataCollection(EntityMetadataList? baseCollection = null)
            : base(baseCollection ?? new())
        {
        }
#pragma warning restore IDE0028
    }

    [PublicAPI]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public partial class CDSTemplate
    {
        [PublicAPI]
        public string? Namespace { get; init; }

        [PublicAPI]
        public string TypeName { get; set; }

        [PublicAPI]
        public EntityMetadataCollection Metadata { get; private set; }

#pragma warning disable IDE0028
        [SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
        [SuppressMessage("Style", "IDE0290:Use primary constructor",
            Justification = "Not supported on older .NET versions.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression",
            Justification = "It is not unnecessary but rather ReSharper only.")]
        [SuppressMessage("ReSharper", "UseCollectionExpression")]
        public CDSTemplate(
            EntityMetadataCollection? metadata = null,
            string? ns = null,
            string? typeName = null)
        {
            Metadata = metadata ?? new EntityMetadataCollection();

            TypeName = typeName ?? "InvalidType";
            Namespace = ns ?? "InvalidNamespace";
        }
#pragma warning restore IDE0028

        [PublicAPI]
        public static string TransformText(
            EntityMetadataCollection? metadata,
            string? ns,
            string? typeName)
        {
            var template = new CDSTemplate(metadata, ns, typeName);
            return template.TransformText();
        }
    }
}
