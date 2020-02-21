using Microsoft.Extensions.Configuration;

namespace ApiExampleProject.IntegrationTests.Configuration
{
    public static class ConfigurationHelper
    {
        private static IntegrationTestSettings integrationTestSettings;

        public static IntegrationTestSettings Settings
        {
            get
            {
                if (integrationTestSettings != null)
                {
                    return integrationTestSettings;
                }

                var configurationRoot = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();

                integrationTestSettings = new IntegrationTestSettings();
                configurationRoot.Bind(integrationTestSettings);

                return integrationTestSettings;
            }
        }
    }
}
