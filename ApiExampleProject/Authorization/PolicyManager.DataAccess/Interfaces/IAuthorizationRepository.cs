using System.Collections.Generic;
using System.Threading.Tasks;
using PolicyManager.DataAccess.Models;
using Group = Microsoft.Graph.Group;

namespace PolicyManager.DataAccess.Interfaces
{
    public interface IAuthorizationRepository
    {
        Task<IEnumerable<PolicyResult>> EvaluateAsync(InitialState<Group> initialState);
    }
}
