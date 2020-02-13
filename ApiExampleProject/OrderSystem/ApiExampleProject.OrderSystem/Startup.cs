using System;
using ApiExampleProject.Authentication.Extensions;
using ApiExampleProject.Common.Interfaces;
using ApiExampleProject.Common.Validators;
using ApiExampleProject.Documentation;
using ApiExampleProject.Documentation.Interfaces;
using ApiExampleProject.OrderSystem;
using ApiExampleProject.OrderSystem.DataAccess.Extensions;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace ApiExampleProject.OrderSystem
{
    public class Startup
        : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            _ = builder ?? throw new ArgumentNullException(nameof(builder));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            builder.Services.AddDataAccessDependencies(configuration);
            builder.Services.AddTokenValidatorDependencies(configuration);
            builder.Services.AddSingleton<IJsonHttpContentValidator, JsonHttpContentValidator>();
            builder.Services.AddScoped<IDocumentationRepository, DocumentationRepository>();
        }
    }
}
