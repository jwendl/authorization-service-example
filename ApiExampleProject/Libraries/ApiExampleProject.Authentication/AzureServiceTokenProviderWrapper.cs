using System.Threading.Tasks;
using ApiExampleProject.Authentication.Interfaces;
using Microsoft.Azure.Services.AppAuthentication;

namespace ApiExampleProject.Authentication
{
    public class AzureServiceTokenProviderWrapper
        : IAzureServiceTokenProviderWrapper
    {
        private readonly AzureServiceTokenProvider azureServiceTokenProvider;

        public AzureServiceTokenProviderWrapper()
        {
            azureServiceTokenProvider = new AzureServiceTokenProvider();
        }

        public async Task<string> GetAccessTokenAsync(string resource)
        {
            return await azureServiceTokenProvider.GetAccessTokenAsync(resource);
        }
    }
}
