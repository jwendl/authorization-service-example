using System.Threading.Tasks;
using PolicyManager.DataAccess.Models;
using Refit;

namespace PolicyManager.Client
{
    public interface IPolicyManagerServiceClient
    {
        [Headers("Authorization: Bearer")]
        [Post("/api/things")]
        Task<Thing> CreateThingAsync([Body] Thing thing);
    }
}
