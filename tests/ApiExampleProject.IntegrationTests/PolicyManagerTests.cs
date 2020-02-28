using System;
using System.Threading.Tasks;
using ApiExampleProject.IntegrationTests.TestFixtures;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PolicyManager.Client;
using PolicyManager.Client.Extensions;
using Xunit;

namespace ApiExampleProject.IntegrationTests
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    [Collection(nameof(PolicyManagerTestCollection))]
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
    }
}
