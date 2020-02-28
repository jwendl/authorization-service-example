using Xunit;

namespace ApiExampleProject.IntegrationTests.TestFixtures
{
    [CollectionDefinition(nameof(PolicyManagerTestCollection))]
    public class PolicyManagerTestCollection
        : ICollectionFixture<PolicyManagerTestFixture>
    {

    }
}
