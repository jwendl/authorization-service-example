using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Authentication.Resources;
using ApiExampleProject.Common.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace ApiExampleProject.Authentication
{
    public class TokenValidator
        : ITokenValidator
    {
        private readonly ILogger<TokenValidator> logger;
        private readonly TokenValidatorConfiguration tokenValidatorConfiguration;
        private readonly ISecurityTokenValidator securityTokenValidator;
        private readonly IConfigurationManager<OpenIdConnectConfiguration> configurationManager;

        public TokenValidator(ISecurityTokenValidator securityTokenValidator, IConfigurationManager<OpenIdConnectConfiguration> configurationManager, IOptions<TokenValidatorConfiguration> tokenValidatorOptions, ILogger<TokenValidator> logger)
        {
            this.securityTokenValidator = securityTokenValidator ?? throw new ArgumentNullException(nameof(securityTokenValidator));
            this.configurationManager = configurationManager ?? throw new ArgumentNullException(nameof(configurationManager));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _ = tokenValidatorOptions ?? throw new ArgumentNullException(nameof(tokenValidatorOptions));
            tokenValidatorConfiguration = tokenValidatorOptions.Value;
        }

        private TokenValidationParameters ValidationParameters { get; set; }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(AuthenticationHeaderValue authenticationHeaderValue)
        {
            _ = authenticationHeaderValue ?? throw new ArgumentNullException(nameof(authenticationHeaderValue));

            if (!string.Equals(authenticationHeaderValue.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase))
            {
                logger.LogError($"Invalid jwt token schema {authenticationHeaderValue.Scheme}");
                return null;
            }

            return await ValidateTokenAsync(authenticationHeaderValue.Parameter);
        }

        public async Task<ClaimsPrincipal> ValidateTokenAsync(string token)
        {
            if (!securityTokenValidator.CanReadToken(token))
            {
                logger.LogError(AuthenticationResources.TokenInvalid);
                return null;
            }

            if (ValidationParameters == null)
            {
                var openIdConnectConfiguration = await configurationManager.GetConfigurationAsync(CancellationToken.None);
                ValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKeys = openIdConnectConfiguration.SigningKeys,
                    RequireSignedTokens = true,
                    ValidAudiences = tokenValidatorConfiguration.ValidAudiences,
                    ValidateAudience = true,
                    ValidIssuers = tokenValidatorConfiguration.ValidIssuers,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                };
            }

            ClaimsPrincipal claimsPrincipal = null;
            try
            {
                logger.LogInformation(tokenValidatorConfiguration.Audience);
                logger.LogInformation(tokenValidatorConfiguration.Authority);
                logger.LogInformation(tokenValidatorConfiguration.ClientId);
                claimsPrincipal = securityTokenValidator.ValidateToken(token, ValidationParameters, out _);
            }
            catch (SecurityTokenException ex)
            {
                logger.LogError(ex.Message);
            }

            return claimsPrincipal;
        }
    }
}
