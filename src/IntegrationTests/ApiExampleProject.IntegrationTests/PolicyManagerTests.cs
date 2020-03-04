using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PolicyManager.Client;
using PolicyManager.Client.Extensions;
using PolicyManager.DataAccess.Models;
using System;
using System.Linq;
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
            createdThing.Id.Should().NotBeEmpty();
            createdThing.Name.Should().Be(thing.Name);
            createdThing.Identifier.Should().Be(thing.Identifier);
            createdThing.Description.Should().Be(thing.Description);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task CreateThingAttributeAsync()
        {
            var policyManagerClient = serviceProvider.GetRequiredService<IPolicyManagerServiceClient>();
            var thing = new Thing()
            {
                Name = "Test Thing 2",
                Identifier = "/api/test2",
                Description = "This is for integration testing.",
            };

            var createdThing = await policyManagerClient.CreateThingAsync(thing);
            createdThing.Id.Should().NotBeEmpty();
            createdThing.Name.Should().Be(thing.Name);
            createdThing.Identifier.Should().Be(thing.Identifier);
            createdThing.Description.Should().Be(thing.Description);

            var thingAttribute = new ThingAttribute()
            {
                ThingId = createdThing.Id,
                Key = "Location",
                Value = "Washington",
            };

            var createdThingAttribute = await policyManagerClient.CreateThingAttributeAsync(thingAttribute);
            createdThingAttribute.Id.Should().NotBeEmpty();
            createdThingAttribute.Key.Should().Be(thingAttribute.Key);
            createdThingAttribute.Value.Should().Be(thingAttribute.Value);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task CreateThingPolicy()
        {
            var policyManagerClient = serviceProvider.GetRequiredService<IPolicyManagerServiceClient>();
            var thing = new Thing()
            {
                Name = "Test Thing 3",
                Identifier = "/api/test3",
                Description = "This is for integration testing.",
            };

            var createdThing = await policyManagerClient.CreateThingAsync(thing);
            createdThing.Id.Should().NotBeEmpty();
            createdThing.Name.Should().Be(thing.Name);
            createdThing.Identifier.Should().Be(thing.Identifier);
            createdThing.Description.Should().Be(thing.Description);

            var thingPolicy = new ThingPolicy()
            {
                ThingId = createdThing.Id,
                Name = "User is Justin",
                Description = "Checks if our logged in user is justin",
                Expression = "userPrincipalName = \"live.com#jwendl@hotmail.com\"",
            };

            var createdThingPolicy = await policyManagerClient.CreateThingPoliciesAsync(thingPolicy);
            createdThingPolicy.Id.Should().NotBeEmpty();
            createdThingPolicy.Name.Should().Be(thingPolicy.Name);
            createdThingPolicy.Description.Should().Be(thingPolicy.Description);
            createdThingPolicy.Expression.Should().Be(thingPolicy.Expression);
        }

        [Fact]
        [Trait("Category", "Integration")]
        public async Task CheckAccess()
        {
            var policyManagerClient = serviceProvider.GetRequiredService<IPolicyManagerServiceClient>();
            var thing = new Thing()
            {
                Name = "Test Thing 3",
                Identifier = "/api/test3",
                Description = "This is for integration testing.",
            };

            var createdThing = await policyManagerClient.CreateThingAsync(thing);
            createdThing.Id.Should().NotBeEmpty();
            createdThing.Name.Should().Be(thing.Name);
            createdThing.Identifier.Should().Be(thing.Identifier);
            createdThing.Description.Should().Be(thing.Description);

            var thingPolicy = new ThingPolicy()
            {
                ThingId = createdThing.Id,
                Name = "User is Justin",
                Description = "Checks if our logged in user is justin",
                Expression = "userPrincipalName = \"live.com#jwendl@hotmail.com\"",
            };

            var createdThingPolicy = await policyManagerClient.CreateThingPoliciesAsync(thingPolicy);
            createdThingPolicy.Id.Should().NotBeEmpty();
            createdThingPolicy.Name.Should().Be(thingPolicy.Name);
            createdThingPolicy.Description.Should().Be(thingPolicy.Description);
            createdThingPolicy.Expression.Should().Be(thingPolicy.Expression);

            var policyAccessResults = await policyManagerClient.CheckAccessAsync(new CheckAccessRequest()
            {
                RequestIdentifier = "/api/test3",
            });
            policyAccessResults.Should().HaveCount(1);
        }
    }
}
