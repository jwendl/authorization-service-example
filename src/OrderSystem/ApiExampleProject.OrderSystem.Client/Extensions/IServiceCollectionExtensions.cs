using System;
using ApiExampleProject.Authentication;
using ApiExampleProject.Authentication.Handlers;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Configuration;
using ApiExampleProject.Common.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace ApiExampleProject.OrderSystem.Client.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddClientDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var clientConfiguration = new ClientConfiguration();
            configuration.Bind("ClientConfiguration", clientConfiguration);
            serviceCollection.Configure<ClientConfiguration>(cc => configuration.Bind("ClientConfiguration", cc));
            serviceCollection.Configure<TokenCreatorConfiguration>(tcc => configuration.Bind("TokenCreator", tcc));

            serviceCollection.AddSingleton<ITokenCreator, TokenCreator>();
            var asyncRetryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempts => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempts)));

            serviceCollection.AddSingleton<HttpLoggingHandler>();
            serviceCollection.AddSingleton<ServiceToServiceAuthenticationMessageHandler>();

            serviceCollection.AddRefitClient<IOrderServiceClient>()
                .AddPolicyHandler(asyncRetryPolicy)
                .AddHttpMessageHandler<ServiceToServiceAuthenticationMessageHandler>()
                .AddHttpMessageHandler<HttpLoggingHandler>()
                .ConfigureHttpClient(http => http.BaseAddress = clientConfiguration.ApiServiceUri);

            return serviceCollection;
        }
    }
}
