using System;
using ApiExampleProject.Authentication;
using ApiExampleProject.Authentication.Extensions;
using ApiExampleProject.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PolicyManager.DataAccess.Interfaces;
using PolicyManager.DataAccess.Repositories;

namespace PolicyManager.DataAccess.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            serviceCollection.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            serviceCollection.AddScoped<IDataContext, DataContext>();
            serviceCollection.AddSingleton<IAzureServiceTokenProviderWrapper, AzureServiceTokenProviderWrapper>();

            serviceCollection.AddTokenCreatorDependencies(configuration);
            serviceCollection.AddSingleton<IMicrosoftGraphRepository, MicrosoftGraphRepository>();
            serviceCollection.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));

            serviceCollection.AddScoped<IAuthorizationRepository, AuthorizationRepository>();

            return serviceCollection;
        }
    }
}
