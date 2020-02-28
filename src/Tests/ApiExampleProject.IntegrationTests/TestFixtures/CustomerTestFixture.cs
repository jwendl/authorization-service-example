using ApiExampleProject.IntegrationTests.Configuration;

namespace ApiExampleProject.IntegrationTests.TestFixtures
{
    public class CustomerTestFixture
        : BaseTestFixture
    {
        public CustomerTestFixture()
            : base(ConfigurationHelper.Settings.CustomerDataApplicationPath, 7002)
        {

        }
    }
}
