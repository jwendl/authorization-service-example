using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Constants;
using ApiExampleProject.Common.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.OpenApi.Models;
using PolicyManager.DataAccess.Interfaces;
using PolicyManager.DataAccess.Models;
using PolicyManager.Models;
using PolicyManager.Resources;
using PolicyManager.Validators;

namespace PolicyManager
{
    public class CheckAccessFunctions
    {
        private readonly ITokenValidator tokenValidator;
        private readonly IJsonHttpContentValidator jsonHttpContentValidator;
        private readonly IMicrosoftGraphRepository microsoftGraphRepository;
        private readonly IAuthorizationRepository authorizationRepository;

        public CheckAccessFunctions(ITokenValidator tokenValidator, IJsonHttpContentValidator jsonHttpContentValidator, IMicrosoftGraphRepository microsoftGraphRepository, IAuthorizationRepository authorizationRepository)
        {
            this.tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
            this.jsonHttpContentValidator = jsonHttpContentValidator ?? throw new ArgumentNullException(nameof(jsonHttpContentValidator));
            this.microsoftGraphRepository = microsoftGraphRepository ?? throw new ArgumentNullException(nameof(microsoftGraphRepository));
            this.authorizationRepository = authorizationRepository ?? throw new ArgumentNullException(nameof(authorizationRepository));
        }

        [FunctionName(nameof(CheckAccess))]
        [OpenApiOperation(nameof(CheckAccess), "Check Access", Description = "Checks access to a resource.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(IEnumerable<PolicyResult>), Description = "The new environment object.")]
        [OpenApiResponseBody(HttpStatusCode.Created, ContentTypes.Application.Json, typeof(IEnumerable<PolicyResult>))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> CheckAccess([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "checkAccess")] HttpRequestMessage req, ILogger log)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(PolicyManagerResources.CheckAccessStartLog);

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(req.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var jsonValidationResult = await jsonHttpContentValidator.ValidateJsonAsync<CheckAccessRequest, CheckAccessRequestValidator>(req.Content);
            if (!jsonValidationResult.IsValid)
            {
                return jsonValidationResult.Message;
            }

            var groups = await microsoftGraphRepository.FetchMyGroupsAsync(req.Headers.Authorization);
            var initialState = new InitialState<Group>()
            {
                ClaimsPrincipal = claimsPrincipal,
                Identifier = jsonValidationResult.Item.RequestIdentifier,
                Groups = groups,
            };

            var policyResults = await authorizationRepository.EvaluateAsync(req.Headers.Authorization, initialState);
            log.LogInformation(PolicyManagerResources.CheckAccessEndLog);

            var content = new StringContent(JsonSerializer.Serialize(policyResults), Encoding.UTF8, ContentTypes.Application.Json);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        }
    }
}
