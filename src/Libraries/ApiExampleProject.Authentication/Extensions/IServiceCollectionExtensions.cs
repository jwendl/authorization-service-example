using System;
using System.IdentityModel.Tokens.Jwt;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace ApiExampleProject.Authentication.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddTokenValidatorDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            serviceCollection.Configure<TokenValidatorConfiguration>(tvc => configuration.Bind("TokenValidator", tvc));
            serviceCollection.AddSingleton<IConfigurationManager<OpenIdConnectConfiguration>, ConfigurationManager<OpenIdConnectConfiguration>>((serviceProvider) =>
            {
                var tokenManagerConfigOptions = serviceProvider.GetRequiredService<IOptions<TokenValidatorConfiguration>>();
                var tokenManagerConfig = tokenManagerConfigOptions.Value;
                var metadataAddress = $"{tokenManagerConfig.AuthorityUri}/.well-known/openid-configuration";
                return new ConfigurationManager<OpenIdConnectConfiguration>(metadataAddress, new OpenIdConnectConfigurationRetriever());
            });
            serviceCollection.AddSingleton<ISecurityTokenValidator, JwtSecurityTokenHandler>();
            serviceCollection.AddSingleton<ITokenValidator, TokenValidator>();

            return serviceCollection;
        }

        public static IServiceCollection AddTokenCreatorDependencies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));

            serviceCollection.Configure<TokenCreatorConfiguration>(tcc => configuration.Bind("TokenCreator", tcc));
            serviceCollection.AddSingleton<ITokenCreator, TokenCreator>();
            serviceCollection.AddSingleton<AbstractApplicationBuilder<ConfidentialClientApplicationBuilder>, ConfidentialClientApplicationBuilder>((sp) =>
            {
                var options = sp.GetRequiredService<IOptions<TokenCreatorConfiguration>>();
                var tokenCreatorConfiguration = options.Value;

                return ConfidentialClientApplicationBuilder.Create(tokenCreatorConfiguration.ClientId)
                    .WithClientSecret(tokenCreatorConfiguration.ClientSecret);
            });

            return serviceCollection;
        }
    }
}
