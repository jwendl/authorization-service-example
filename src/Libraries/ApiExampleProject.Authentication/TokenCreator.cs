using System;
using System.Diagnostics.CodeAnalysis;
using System.Security;
using System.Threading.Tasks;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Authentication.Resources;
using ApiExampleProject.Common.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace ApiExampleProject.Authentication
{
    [ExcludeFromCodeCoverage()]
    public class TokenCreator
        : ITokenCreator
    {
        private readonly TokenCreatorConfiguration tokenCreatorConfiguration;
        private readonly AbstractApplicationBuilder<ConfidentialClientApplicationBuilder> confidentialClientApplicationBuilder;
        private readonly AbstractApplicationBuilder<PublicClientApplicationBuilder> publicClientApplicationBuilder;
        private readonly ILogger<TokenCreator> logger;

        public TokenCreator(IOptions<TokenCreatorConfiguration> options, AbstractApplicationBuilder<ConfidentialClientApplicationBuilder> confidentialClientApplicationBuilder, AbstractApplicationBuilder<PublicClientApplicationBuilder> publicClientApplicationBuilder, ILogger<TokenCreator> logger)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));
            this.confidentialClientApplicationBuilder = confidentialClientApplicationBuilder ?? throw new ArgumentNullException(nameof(confidentialClientApplicationBuilder));
            this.publicClientApplicationBuilder = publicClientApplicationBuilder ?? throw new ArgumentNullException(nameof(publicClientApplicationBuilder));

            tokenCreatorConfiguration = options.Value;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetIntegrationTestTokenAsync()
        {
            using var securePassword = new SecureString();
            foreach (var c in tokenCreatorConfiguration.TestPassword)
            {
                securePassword.AppendChar(c);
            }

            try
            {
                var publicClientApplication = publicClientApplicationBuilder
                    .WithTenantId(tokenCreatorConfiguration.TenantId.ToString())
                    .Build();

                var scopes = tokenCreatorConfiguration.Scopes.Split(' ');
                var authenticationResult = await publicClientApplication
                    .AcquireTokenByUsernamePassword(scopes, tokenCreatorConfiguration.TestUsername, securePassword)
                    .ExecuteAsync();
                return authenticationResult.AccessToken;
            }
            catch (MsalServiceException msalServiceException)
            {
                logger.LogError(msalServiceException, msalServiceException.Message);
                throw new InvalidOperationException(AuthenticationResources.MsalException, msalServiceException);
            }
        }

        public async Task<string> GetUserBasedAccessTokenAsync()
        {
            try
            {
                var publicClientApplication = publicClientApplicationBuilder
                    .WithTenantId(tokenCreatorConfiguration.TenantId.ToString())
                    .Build();

                var scopes = tokenCreatorConfiguration.Scopes.Split(' ');
                var authenticationResult = await publicClientApplication
                    .AcquireTokenInteractive(scopes)
                    .ExecuteAsync();
                return authenticationResult.AccessToken;
            }
            catch (MsalServiceException msalServiceException)
            {
                logger.LogError(msalServiceException, msalServiceException.Message);
                throw new InvalidOperationException(AuthenticationResources.MsalException, msalServiceException);
            }
        }

        public async Task<string> GetClientApplicationAccessTokenAsync()
        {
            try
            {
                var scopes = tokenCreatorConfiguration.Scopes.Split(' ');
                var confidentialClientApplication = confidentialClientApplicationBuilder
                    .WithTenantId(tokenCreatorConfiguration.TenantId.ToString())
                    .Build();

                var authenticationResult = await confidentialClientApplication.AcquireTokenForClient(scopes)
                    .ExecuteAsync();
                return authenticationResult.AccessToken;
            }
            catch (MsalServiceException msalServiceException)
            {
                logger.LogError(msalServiceException, msalServiceException.Message);
                throw new InvalidOperationException(AuthenticationResources.MsalException, msalServiceException);
            }
        }

        public async Task<string> GetAccessTokenOnBehalfOf(string userAssertionToken)
        {
            var scopes = tokenCreatorConfiguration.Scopes.Split(' ');
            var userAssertion = new UserAssertion(userAssertionToken);
            var confidentialClientApplication = confidentialClientApplicationBuilder
                .WithTenantId(tokenCreatorConfiguration.TenantId.ToString())
                .Build();

            var acquireTokenOnBehalfOfBuilder = confidentialClientApplication.AcquireTokenOnBehalfOf(scopes, userAssertion);

            try
            {
                var authenticationResult = await acquireTokenOnBehalfOfBuilder.ExecuteAsync();
                return authenticationResult.AccessToken;
            }
            catch (MsalServiceException msalServiceException)
            {
                logger.LogError(msalServiceException, msalServiceException.Message);
                throw new InvalidOperationException(AuthenticationResources.MsalException, msalServiceException);
            }
        }
    }
}
