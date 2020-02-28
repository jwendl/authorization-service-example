using System;
using System.Threading.Tasks;
using ApiExampleProject.CustomerData.Client;
using ApiExampleProject.CustomerData.Client.Extensions;
using ApiExampleProject.CustomerData.DataAccess.Models;
using ApiExampleProject.IntegrationTests.TestFixtures;
using Bogus;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ApiExampleProject.IntegrationTests
{
    [Trait("Category", "SkipWhenLiveUnitTesting")]
    [Collection(nameof(CustomerTestCollection))]
    public class CustomerFunctionsTests
    {
        private readonly IServiceProvider serviceProvider;

        public CustomerFunctionsTests()
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
        public async Task CreateCustomer()
        {
            var customerGenerator = new Faker<Customer>()
                .RuleFor(c => c.FirstName, p => p.Person.FirstName)
                .RuleFor(c => c.LastName, p => p.Person.LastName)
                .RuleFor(c => c.BirthDate, p => p.Person.DateOfBirth);
            var expectedCustomer = customerGenerator.Generate();

            var customerClient = serviceProvider.GetRequiredService<ICustomerServiceClient>();
            var createdCustomer = await customerClient.CreateCustomerAsync(expectedCustomer);

            createdCustomer.Id.Should().NotBeEmpty();
            createdCustomer.FirstName.Should().Be(expectedCustomer.FirstName);
            createdCustomer.LastName.Should().Be(expectedCustomer.LastName);
            createdCustomer.BirthDate.Should().Be(expectedCustomer.BirthDate);
        }
    }
}
