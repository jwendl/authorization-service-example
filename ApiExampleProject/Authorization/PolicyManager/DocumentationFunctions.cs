using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;
using ApiExampleProject.Common.Constants;
using ApiExampleProject.Documentation.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace PolicyManager
{
    public class DocumentationFunctions
    {
        private const string DocumentName = "openapi.json";
        private const string AuthenticationParameter = "code";
        private readonly IDocumentationRepository documentationRepository;

        public DocumentationFunctions(IDocumentationRepository documentationRepository)
        {
            this.documentationRepository = documentationRepository ?? throw new ArgumentNullException(nameof(documentationRepository));
        }

        [OpenApiIgnore]
        [FunctionName(nameof(GenerateJsonDocumentation))]
        public async Task<IActionResult> GenerateJsonDocumentation([HttpTrigger(AuthorizationLevel.Function, "get", Route = DocumentName)] HttpRequest req)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            var result = await documentationRepository.GetSwaggerDocumentContentAsync(req, Assembly.GetExecutingAssembly());
            var response = new ContentResult()
            {
                Content = result,
                ContentType = ContentTypes.Application.Json,
                StatusCode = (int)HttpStatusCode.OK
            };

            return response;
        }

        [OpenApiIgnore]
        [FunctionName(nameof(GenerateDocumentationInterface))]
        public async Task<IActionResult> GenerateDocumentationInterface([HttpTrigger(AuthorizationLevel.Function, "get", Route = "docs")] HttpRequest req)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            var authCode = req.Query[AuthenticationParameter];
            var result = await documentationRepository.GetSwaggerUIContentAsync(req, DocumentName, authCode, Assembly.GetExecutingAssembly());
            var response = new ContentResult()
            {
                Content = result,
                ContentType = ContentTypes.TextType.Html,
                StatusCode = (int)HttpStatusCode.OK
            };

            return response;
        }
    }
}
