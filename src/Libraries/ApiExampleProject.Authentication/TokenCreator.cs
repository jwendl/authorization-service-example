using System;
using System.Diagnostics.CodeAnalysis;
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
        private readonly IConfidentialClientApplication confidentialClientApplication;
        private readonly ILogger<TokenCreator> logger;

        public TokenCreator(IOptions<TokenCreatorConfiguration> options, AbstractApplicationBuilder<ConfidentialClientApplicationBuilder> abstractApplicationBuilder, ILogger<TokenCreator> logger)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));
            _ = abstractApplicationBuilder ?? throw new ArgumentNullException(nameof(abstractApplicationBuilder));

            tokenCreatorConfiguration = options.Value;
            confidentialClientApplication = abstractApplicationBuilder
                .WithTenantId(tokenCreatorConfiguration.TenantId.ToString())
                .Build();

            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetAccessTokenAsync()
        {
            try
            {
                var scopes = tokenCreatorConfiguration.Scopes.Split(' ');
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
