using System;
using System.Data.SqlClient;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PolicyManager.DataAccess.Interfaces;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess
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

        public DbSet<Thing> Things { get; set; }

        public DbSet<ThingAttribute> ThingAttributes { get; set; }

        public DbSet<ThingPolicy> ThingPolicies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<UserAttribute> UserAttributes { get; set; }

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
                optionsBuilder.UseInMemoryDatabase("PolicyManager");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            _ = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));

            // User
            modelBuilder.Entity<User>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<User>()
                .HasKey(u => u.Id);

            // User Attribute
            modelBuilder.Entity<UserAttribute>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<UserAttribute>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<UserAttribute>()
                .HasOne(ua => ua.User)
                .WithMany(u => u.UserAttributes)
                .HasForeignKey(ua => ua.UserId);

            // Resource
            modelBuilder.Entity<Thing>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Thing>()
                .HasKey(t => t.Id);

            // Resource Attribute
            modelBuilder.Entity<ThingAttribute>()
                .Property(t => t.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ThingAttribute>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<ThingAttribute>()
                .HasOne(ta => ta.Thing)
                .WithMany(t => t.ThingAttributes)
                .HasForeignKey(ta => ta.ThingId);

            // Resource Policy
            modelBuilder.Entity<ThingPolicy>()
                .Property(c => c.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<ThingPolicy>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<ThingPolicy>()
                .HasOne(tp => tp.Thing)
                .WithMany(t => t.ThingPolicies)
                .HasForeignKey(tp => tp.ThingId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
