using System;
using ApiExampleProject.Authentication;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Configuration;
using ApiExampleProject.OrderSystem.DataAccess.Interfaces;
using ApiExampleProject.OrderSystem.DataAccess.Repositories;
using Microsoft.Azure.Cosmos.Fluent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace ApiExampleProject.OrderSystem.DataAccess.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            serviceCollection.Configure<CosmosConfiguration>(cc => configuration.Bind("CosmosConfiguration", cc));
            serviceCollection.AddSingleton<IAzureServiceTokenProviderWrapper, AzureServiceTokenProviderWrapper>();
            serviceCollection.AddSingleton((sp) =>
            {
                var options = sp.GetRequiredService<IOptions<CosmosConfiguration>>();
                var cosmosConfiguration = options.Value;

                var cosmosClientBuilder = new CosmosClientBuilder(cosmosConfiguration.EndpointLocation, cosmosConfiguration.PrimaryKey);
                return cosmosClientBuilder
                        .Build();
            });

            serviceCollection.AddSingleton(typeof(ICosmosClientWrapper<>), typeof(CosmosClientWrapper<>));
            serviceCollection.AddSingleton(typeof(IDataRepository<>), typeof(DataRepository<>));

            return serviceCollection;
        }
    }
}
