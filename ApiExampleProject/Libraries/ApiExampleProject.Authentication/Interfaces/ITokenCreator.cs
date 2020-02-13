using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiExampleProject.Authentication.Interfaces
{
    public interface ITokenCreator
    {
        Task<string> GetAccessTokenAsync();

        Task<string> GetAccessTokenOnBehalfOf(string userAssertionToken);
    }
}
