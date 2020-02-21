using System;
using System.Data.SqlClient;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Configuration;
using ApiExampleProject.CustomerData.DataAccess.Interfaces;
using ApiExampleProject.CustomerData.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace ApiExampleProject.CustomerData.DataAccess
{
    public class DataContext
        : DbContext, IDataContext
    {
        private readonly SqlConnectionConfiguration sqlConnectionConfiguration;
        private readonly IAzureServiceTokenProviderWrapper azureServiceTokenProviderWrapper;

        public DataContext()
        {

        }

        public DataContext(IOptions<SqlConnectionConfiguration> options, IAzureServiceTokenProviderWrapper azureServiceTokenProviderWrapper)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));

            sqlConnectionConfiguration = options.Value;
            this.azureServiceTokenProviderWrapper = azureServiceTokenProviderWrapper;
        }

        public DbSet<Customer> Customers { get; set; }

        protected override async void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!string.IsNullOrWhiteSpace(sqlConnectionConfiguration?.ConnectionString))
            {
                optionsBuilder.UseSqlServer(sqlConnectionConfiguration.ConnectionString);
                var accessToken = await azureServiceTokenProviderWrapper.GetAccessTokenAsync("https://database.windows.net/");

                var sqlConnection = base.Database.GetDbConnection() as SqlConnection;
                _ = sqlConnection ?? throw new NullReferenceException(nameof(sqlConnection));

                sqlConnection.AccessToken = accessToken;
            }
            else
            {
                optionsBuilder.UseInMemoryDatabase("Customer");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            modelBuilder.Entity<Customer>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }
    }
}
