using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi;
using Aliencube.AzureFunctions.Extensions.OpenApi.Configurations;
using Aliencube.AzureFunctions.Extensions.OpenApi.Extensions;
using ApiExampleProject.Documentation.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi;
using Microsoft.OpenApi.Models;

namespace ApiExampleProject.Documentation
{
    public class DocumentationRepository
        : IDocumentationRepository
    {
        private const string RoutePrefix = "api";

        public async Task<string> GetSwaggerDocumentContentAsync(HttpRequest httpRequest, Assembly assembly)
        {
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));

            var fileVersionInformation = FileVersionInfo.GetVersionInfo(assembly.Location);
            var documentHelper = new DocumentHelper(new RouteConstraintFilter());
            var document = new Document(documentHelper);
            var result = await document.InitialiseDocument()
                .AddMetadata(new OpenApiInfo()
                {
                    Title = fileVersionInformation.ProductName,
                    Description = fileVersionInformation.Comments,
                    Contact = new OpenApiContact()
                    {
                        Name = fileVersionInformation.CompanyName
                    },
                    Version = fileVersionInformation.FileVersion,
                })
                .AddServer(httpRequest, RoutePrefix)
                .Build(assembly)
                .RenderAsync(OpenApiSpecVersion.OpenApi2_0, OpenApiFormat.Json);

            return result;
        }

        public async Task<string> GetSwaggerUIContentAsync(HttpRequest httpRequest, string documentName, string authCode, Assembly assembly)
        {
            _ = assembly ?? throw new ArgumentNullException(nameof(assembly));

            var fileVersionInformation = FileVersionInfo.GetVersionInfo(assembly.Location);
            var swaggerUi = new SwaggerUI();
            var result = await swaggerUi
                .AddMetadata(new OpenApiInfo()
                {
                    Title = fileVersionInformation.ProductName,
                    Description = fileVersionInformation.Comments,
                    Contact = new OpenApiContact()
                    {
                        Name = fileVersionInformation.CompanyName
                    },
                    Version = fileVersionInformation.FileVersion,
                })
                .AddServer(httpRequest, RoutePrefix)
                .BuildAsync()
                .RenderAsync(documentName, authCode);

            return result;
        }
    }
}
