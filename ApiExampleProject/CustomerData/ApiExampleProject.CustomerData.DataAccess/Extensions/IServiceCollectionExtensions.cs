using System;
using ApiExampleProject.Authentication;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.CustomerData.DataAccess.Interfaces;
using ApiExampleProject.CustomerData.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiExampleProject.CustomerData.DataAccess.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddDataAccessDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            serviceCollection.AddDbContext<DataContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            serviceCollection.AddScoped<IDataContext, DataContext>();
            serviceCollection.AddSingleton<IAzureServiceTokenProviderWrapper, AzureServiceTokenProviderWrapper>();
            serviceCollection.AddScoped(typeof(IDataRepository<>), typeof(DataRepository<>));

            return serviceCollection;
        }
    }
}
