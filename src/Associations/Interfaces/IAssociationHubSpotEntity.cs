using Skarp.HubSpotClient.Core;

namespace Skarp.HubSpotClient.Associations.Interfaces
{
    // not using IHubSpotEntity because the api does not map to and from with property values. Mostly uses just ids
    public interface IAssociationHubSpotEntity
    {
        long FromObjectId { get; set; }
        long ToObjectId { get; set; }
        string Category { get; }
        int DefinitionId { get; set; }
        string RouteBasePath { get; }

    }
}
