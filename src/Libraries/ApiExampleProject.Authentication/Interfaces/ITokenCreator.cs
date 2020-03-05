using System.Threading.Tasks;

namespace ApiExampleProject.Authentication.Interfaces
{
    public interface ITokenCreator
    {
        Task<string> GetIntegrationTestTokenAsync();

        Task<string> GetUserBasedAccessTokenAsync();

        Task<string> GetClientApplicationAccessTokenAsync();

        Task<string> GetAccessTokenOnBehalfOf(string userAssertionToken);
    }
}
