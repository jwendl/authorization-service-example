using System.Threading.Tasks;

namespace ApiExampleProject.Authentication.Interfaces
{
    public interface IAzureServiceTokenProviderWrapper
    {
        Task<string> GetAccessTokenAsync(string resource);
    }
}
