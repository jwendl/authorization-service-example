using System;
using System.Threading.Tasks;
using ApiExampleProject.IntegrationTests.Configuration;
using ApiExampleProject.IntegrationTests.TestFixtures;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PolicyManager.Client;
using PolicyManager.Client.Extensions;
using PolicyManager.DataAccess.Models;
using Xunit;

namespace ApiExampleProject.IntegrationTests
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    [Collection(nameof(IntegrationTestCollection))]
    public class PolicyManagerTests
    {
        private readonly IntegrationTestFixture integrationTestFixture;
        private readonly IServiceProvider serviceProvider;

        public PolicyManagerTests(IntegrationTestFixture integrationTestFixture)
        {
            this.integrationTestFixture = integrationTestFixture ?? throw new ArgumentNullException(nameof(integrationTestFixture));
            integrationTestFixture.FunctionApplicationPath = ConfigurationHelper.Settings.PolicyManagerApplicationPath;
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
        public async Task CreateThing()
        {
            var thingGenerator = new Faker<Thing>()
                .RuleFor(t => t.Name, p => p.Lorem.Word())
                .RuleFor(t => t.Description, p => p.Rant.Review())
                .RuleFor(t => t.Identifier, p => p.Lorem.Word());
            var expectedThing = thingGenerator.Generate();

            var policyManagerClient = serviceProvider.GetRequiredService<IPolicyManagerServiceClient>();
            var createdThing = await policyManagerClient.CreateThingAsync(expectedThing);

            createdThing.Id.Should().NotBeEmpty();
            createdThing.Name.Should().Be(expectedThing.Name);
            createdThing.Description.Should().Be(expectedThing.Description);
        }
    }
}
