using Xunit;

namespace ApiExampleProject.IntegrationTests.TestFixtures
{
    [CollectionDefinition(nameof(CustomerTestCollection))]
    public class CustomerTestCollection
        : ICollectionFixture<CustomerTestFixture>
    {

    }
}
