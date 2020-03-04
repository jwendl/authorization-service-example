using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using ApiExampleProject.Authentication.Interfaces;

namespace ApiExampleProject.Authentication.Handlers
{
    public class ServiceToServiceAuthenticationMessageHandler
        : DelegatingHandler
    {
        private readonly ITokenCreator tokenCreator;

        public ServiceToServiceAuthenticationMessageHandler(ITokenCreator tokenCreator)
        {
            this.tokenCreator = tokenCreator ?? throw new ArgumentNullException(nameof(tokenCreator));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var httpRequestHeaders = request.Headers;

            // If you have the following attribute in your interface, the authorization header will be "Bearer", not null.
            // [Headers("Authorization: Bearer")]
            // If we have a token, then we want to use that token - otherwise generate a service to service one.
            var authenticationHeaderValue = httpRequestHeaders.Authorization;
            if (authenticationHeaderValue != null && authenticationHeaderValue.Scheme == "Bearer" && !string.IsNullOrWhiteSpace(authenticationHeaderValue.Parameter))
            {
                var accessToken = await tokenCreator.GetAccessTokenOnBehalfOf(authenticationHeaderValue.Parameter);
                httpRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationHeaderValue.Scheme, accessToken);
            }
            else
            {
                var accessToken = await tokenCreator.GetAccessTokenAsync();
                httpRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationHeaderValue.Scheme, accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
