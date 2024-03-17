using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.PowerPlatform.Dataverse.Client.Extensions;
using Microsoft.Xrm.Sdk.Discovery;
using Microsoft.Xrm.Sdk.Metadata;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MarkMpn.FetchXmlToWebAPI;

namespace NY.Dataverse.LINQPadDriver
{
    public class WebAPIQueryHelper
    {
        public static string? GetWebApiUrl(ServiceClient dataverseClient, string query)
        {
            var url = $"{dataverseClient.ConnectedOrgPublishedEndpoints[EndpointType.WebApplication]}api/data/v{dataverseClient.ConnectedOrgVersion.Major}.{dataverseClient.ConnectedOrgVersion.Minor}";
            var fetchXml = XElement.Parse(query);
            var entityElement = fetchXml.Element(FetchAttributes.Entity);
            var entityMetadata = new List<EntityMetadata>();
            var mainEntity = dataverseClient.GetEntityMetadata(entityElement?.Attribute(FetchAttributes.Name)?.Value, EntityFilters.Entity | EntityFilters.Attributes | EntityFilters.Relationships);
            entityMetadata.Add(mainEntity);

            if (fetchXml.Descendants(FetchAttributes.LinkEntity).Any())
                entityMetadata.AddRange(fetchXml.Descendants(FetchAttributes.LinkEntity).Select(x => dataverseClient.GetEntityMetadata(x.Attribute(FetchAttributes.Name)?.Value, EntityFilters.Entity | EntityFilters.Attributes | EntityFilters.Relationships)).ToList());

            var converter = new FetchXmlToWebAPIConverter(new LINQPadMetadataProvider(entityMetadata), url);
            var webApiUrl = converter.ConvertFetchXmlToWebAPI(query);
            return webApiUrl;
        }
    }
}
