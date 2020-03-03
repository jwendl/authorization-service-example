using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PolicyManager.Client;
using PolicyManager.Client.Extensions;
using PolicyManager.DataAccess.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace ApiExampleProject.IntegrationTests
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    public class PolicyManagerTests
    {
        private readonly IServiceProvider serviceProvider;

        public PolicyManagerTests()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddClientDependencies(configuration);
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task ReadThings()
        {
            var policyManagerClient = serviceProvider.GetRequiredService<IPolicyManagerServiceClient>();
            var things = await policyManagerClient.GetThingsAsync();
            things.Should().BeEmpty();
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task CreateThing()
        {
            var policyManagerClient = serviceProvider.GetRequiredService<IPolicyManagerServiceClient>();
            var thing = new Thing()
            {
                Name = "Test Thing",
                Identifier = "/api/test",
                Description = "This is for integration testing.",
            };

            var createdThing = await policyManagerClient.CreateThingAsync(thing);
            createdThing.Name.Should().Be(thing.Name);
            createdThing.Identifier.Should().Be(thing.Identifier);
            createdThing.Description.Should().Be(thing.Description);
        }
    }
}
