using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Group = Microsoft.Graph.Group;

namespace PolicyManager.DataAccess.Interfaces
{
    public interface IMicrosoftGraphRepository
    {
        Task<User> FetchMeAsync(AuthenticationHeaderValue authenticationHeaderValue);

        Task<IEnumerable<Group>> FetchMyGroupsAsync(AuthenticationHeaderValue authenticationHeaderValue);
    }
}
