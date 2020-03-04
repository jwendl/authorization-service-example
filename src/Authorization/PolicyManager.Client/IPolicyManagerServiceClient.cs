using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyManager.DataAccess.Models;
using Refit;

namespace PolicyManager.Client
{
    public interface IPolicyManagerServiceClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/api/things")]
        Task<IEnumerable<Thing>> GetThingsAsync();

        [Headers("Authorization: Bearer")]
        [Post("/api/things")]
        Task<Thing> CreateThingAsync([Body] Thing thing);

        [Headers("Authorization: Bearer")]
        [Post("/api/thingAttributes")]
        Task<ThingAttribute> CreateThingAttributeAsync([Body] ThingAttribute thingAttribute);

        [Headers("Authorization: Bearer")]
        [Post("/api/thingPolicies")]
        Task<ThingPolicy> CreateThingPoliciesAsync([Body] ThingPolicy thingPolicy);

        [Headers("Authorization: Bearer")]
        [Post("/api/checkAccess")]
        Task<IEnumerable<PolicyResult>> CheckAccessAsync([Body] CheckAccessRequest checkAccessRequest);
    }
}
