using ApiExampleProject.IntegrationTests.Configuration;

namespace ApiExampleProject.IntegrationTests.TestFixtures
{
    public class PolicyManagerTestFixture
        : BaseTestFixture
    {
        public PolicyManagerTestFixture()
            : base(ConfigurationHelper.Settings.PolicyManagerApplicationPath, 7001)
        {

        }
    }
}
