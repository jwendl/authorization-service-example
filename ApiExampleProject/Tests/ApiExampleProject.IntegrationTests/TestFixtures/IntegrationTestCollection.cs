using Xunit;

namespace ApiExampleProject.IntegrationTests.TestFixtures
{
    [CollectionDefinition(nameof(IntegrationTestCollection))]
    public class IntegrationTestCollection
        : ICollectionFixture<IntegrationTestFixture>
    {

    }
}
