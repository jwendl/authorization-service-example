using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Configuration;
using Microsoft.Extensions.Options;

namespace ApiExampleProject.Authentication.Handlers
{
    public class ServiceToServiceAuthenticationMessageHandler
        : DelegatingHandler
    {
        private readonly ITokenCreator tokenCreator;
        private readonly TokenCreatorConfiguration tokenCreatorConfiguration;

        public ServiceToServiceAuthenticationMessageHandler(ITokenCreator tokenCreator, IOptions<TokenCreatorConfiguration> options)
        {
            this.tokenCreator = tokenCreator ?? throw new ArgumentNullException(nameof(tokenCreator));
            _ = options ?? throw new ArgumentNullException(nameof(options));

            tokenCreatorConfiguration = options.Value;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var httpRequestHeaders = request.Headers;

            // If you have the following attribute in your interface, the authorization header will be "Bearer", not null.
            // [Headers("Authorization: Bearer")]
            var authenticationHeaderValue = httpRequestHeaders.Authorization;

            if (authenticationHeaderValue != null)
            {
                var accessToken = await tokenCreator.GetAccessTokenAsync();
                httpRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationHeaderValue.Scheme, accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
