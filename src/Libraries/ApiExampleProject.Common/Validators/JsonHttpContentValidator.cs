using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiExampleProject.Common.Constants;
using ApiExampleProject.Common.Interfaces;
using ApiExampleProject.Common.Models;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace ApiExampleProject.Common.Validators
{
    public class JsonHttpContentValidator
        : IJsonHttpContentValidator
    {
        private readonly IJsonTextSerializer jsonTextSerializer;
        private readonly ILogger<JsonHttpContentValidator> logger;

        public JsonHttpContentValidator(IJsonTextSerializer jsonTextSerializer, ILogger<JsonHttpContentValidator> logger)
        {
            this.jsonTextSerializer = jsonTextSerializer;
            this.logger = logger;
        }

        public async Task<JsonValidationResult<TModel>> ValidateJsonAsync<TModel, TValidator>(HttpContent httpContent)
            where TValidator : AbstractValidator<TModel>, new()
        {
            _ = httpContent ?? throw new ArgumentNullException(nameof(httpContent));

            try
            {
                var contentStream = await httpContent.ReadAsStreamAsync();
                var result = await jsonTextSerializer.DeserializeObjectAsync<TModel>(contentStream);

                if (result == null)
                {
                    var content = new StringContent($"Invalid json, please visit the swagger definition and retry the request.", Encoding.UTF8, ContentTypes.Application.Json);
                    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = content };
                    return new JsonValidationResult<TModel>()
                    {
                        IsValid = false,
                        Message = httpResponseMessage,
                    };
                }

                var validator = new TValidator();
                var validationResults = validator.Validate(result);

                if (validationResults.IsValid)
                {
                    return new JsonValidationResult<TModel>()
                    {
                        Item = result,
                        IsValid = true,
                    };
                }
                else
                {
                    var validationMessages = string.Join(Environment.NewLine, validationResults.Errors);
                    var content = new StringContent($"Validation errors: {validationMessages}", Encoding.UTF8, ContentTypes.Application.Json);
                    var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = content };
                    return new JsonValidationResult<TModel>()
                    {
                        Item = result,
                        IsValid = false,
                        Message = httpResponseMessage,
                    };
                }
            }
            catch (UnsupportedMediaTypeException unsupportedMediaTypeException)
            {
                logger.LogError(unsupportedMediaTypeException.Message, unsupportedMediaTypeException);

                var content = new StringContent($"Invalid json, error from UnsupportedMediaTypeException: {unsupportedMediaTypeException.Message}.", Encoding.UTF8, ContentTypes.Application.Json);
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = content };
                return new JsonValidationResult<TModel>()
                {
                    IsValid = false,
                    Message = httpResponseMessage,
                };
            }
        }
    }
}
