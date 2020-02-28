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

namespace ApiExampleProject.CustomerData.Client.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddClientDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var customerDataClientConfiguration = new ClientConfiguration();
            configuration.Bind("CustomerDataClientConfiguration", customerDataClientConfiguration);
            serviceCollection.Configure<ClientConfiguration>(cc => configuration.Bind("CustomerDataClientConfiguration", cc));
            serviceCollection.AddTokenCreatorDependencies(configuration);

            var asyncRetryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempts => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempts)));

            serviceCollection.AddSingleton<HttpLoggingHandler>();
            serviceCollection.AddSingleton<ServiceToServiceAuthenticationMessageHandler>();

            serviceCollection.AddRefitClient<ICustomerServiceClient>()
                .AddPolicyHandler(asyncRetryPolicy)
                .AddHttpMessageHandler<ServiceToServiceAuthenticationMessageHandler>()
                .AddHttpMessageHandler<HttpLoggingHandler>()
                .ConfigureHttpClient(http => http.BaseAddress = customerDataClientConfiguration.ApiServiceUri);

            return serviceCollection;
        }
    }
}
