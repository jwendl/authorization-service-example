using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using PolicyManager.DataAccess.Models;
using Group = Microsoft.Graph.Group;

namespace PolicyManager.DataAccess.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<IEnumerable<PolicyResult>> EvaluateAsync(AuthenticationHeaderValue authenticationHeaderValue, InitialState<Group> initialState);
    }
}
