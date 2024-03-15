using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace NY.Dataverse.LINQPadDriver
{
    using EntityMetadataList = List<(
        EntityMetadata entityMetadata,
        List<(string attributeName, List<(string Label, int? Value)> options)> optionMetadata)>;

    [PublicAPI]
    public sealed class EntityMetadataCollection : EntityMetadataList
    {
        [SuppressMessage("Style", "IDE0028:Collection initialization can be simplified",
            Justification = "Not supported on older .NET versions.")]
        [SuppressMessage("ReSharper", "ConvertToPrimaryConstructor")]
        [SuppressMessage("Style", "IDE0290:Use primary constructor",
            Justification = "Not supported on older .NET versions.")]
        [SuppressMessage("CodeQuality", "IDE0079:Remove unnecessary suppression",
            Justification = "It is not unnecessary but rather ReSharper only.")]
        [SuppressMessage("ReSharper", "UseCollectionExpression")]
        private EntityMetadataCollection(EntityMetadataList? baseCollection = null)
            : base(baseCollection ?? new EntityMetadataList())
        {
        }

        [PublicAPI]
        public static EntityMetadataCollection? TryCreate(EntityMetadataList? baseCollection = null) =>
        baseCollection is null ? null : new EntityMetadataCollection(baseCollection);

        [PublicAPI]
        public static EntityMetadataCollection Empty() => new();
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

        [SuppressMessage("Style", "IDE0028:Collection initialization can be simplified",
            Justification = "Not supported on older .NET versions.")]
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
            Metadata = metadata ?? EntityMetadataCollection.Empty();
            TypeName = typeName ?? "InvalidType";
            Namespace = ns ?? "InvalidNamespace";
        }
    }
}
