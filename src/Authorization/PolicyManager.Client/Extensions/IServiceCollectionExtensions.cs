using System;
using ApiExampleProject.Authentication.Extensions;
using ApiExampleProject.Authentication.Handlers;
using ApiExampleProject.Common.Configuration;
using ApiExampleProject.Common.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using Refit;

namespace PolicyManager.Client.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddClientDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var policyManagerClientConfiguration = new ClientConfiguration();
            configuration.Bind("PolicyManagerClientConfiguration", policyManagerClientConfiguration);
            serviceCollection.Configure<ClientConfiguration>(cc => configuration.Bind("PolicyManagerClientConfiguration", cc));
            serviceCollection.AddTokenCreatorDependencies(configuration);

            var asyncRetryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempts => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempts)));

            serviceCollection.AddSingleton<HttpLoggingHandler>();
            serviceCollection.AddSingleton<AuthenticationMessageHandler>();

            serviceCollection.AddRefitClient<IPolicyManagerServiceClient>()
                .AddPolicyHandler(asyncRetryPolicy)
                .AddHttpMessageHandler<AuthenticationMessageHandler>()
                .AddHttpMessageHandler<HttpLoggingHandler>()
                .ConfigureHttpClient(http => http.BaseAddress = policyManagerClientConfiguration.ApiServiceUri);

            return serviceCollection;
        }
    }
}
