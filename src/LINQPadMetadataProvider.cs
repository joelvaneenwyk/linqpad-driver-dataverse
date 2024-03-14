using MarkMpn.FetchXmlToWebAPI;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Linq;

namespace Mycoshiro.Dataverse.LINQPad
{
    public class LINQPadMetadataProvider : IMetadataProvider
    {
        public List<EntityMetadata> Metadata { get; private set; }
        public LINQPadMetadataProvider(List<EntityMetadata> metadata) => Metadata = metadata;

        public bool IsConnected => true;

        public EntityMetadata GetEntity(string logicalName)
        {
            return Metadata.Single(x => x.LogicalName == logicalName);
        }

        public EntityMetadata GetEntity(int otc)
        {
            return Metadata.Single(x => x.ObjectTypeCode == otc);
        }
    }
}
