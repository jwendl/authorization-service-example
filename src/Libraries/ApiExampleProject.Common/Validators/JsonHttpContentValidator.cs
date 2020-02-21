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
using Newtonsoft.Json;

namespace ApiExampleProject.Common.Validators
{
    public class JsonHttpContentValidator
        : IJsonHttpContentValidator
    {
        private readonly ILogger<JsonHttpContentValidator> logger;

        public JsonHttpContentValidator(ILogger<JsonHttpContentValidator> logger)
        {
            this.logger = logger;
        }

        public async Task<JsonValidationResult<TModel>> ValidateJsonAsync<TModel, TValidator>(HttpContent httpContent)
            where TValidator : AbstractValidator<TModel>, new()
        {
            try
            {
                var result = await httpContent.ReadAsAsync<TModel>();
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
            catch (JsonReaderException jsonReaderException)
            {
                logger.LogError(jsonReaderException.Message, jsonReaderException);

                var content = new StringContent($"Invalid json, error from JsonReader: {jsonReaderException.Message}.", Encoding.UTF8, ContentTypes.Application.Json);
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = content };
                return new JsonValidationResult<TModel>()
                {
                    IsValid = false,
                    Message = httpResponseMessage,
                };
            }
            catch (JsonSerializationException jsonSerializationException)
            {
                logger.LogError(jsonSerializationException.Message, jsonSerializationException);

                var content = new StringContent($"Invalid json, error from JsonSerializationException: {jsonSerializationException.Message}.", Encoding.UTF8, ContentTypes.Application.Json);
                var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = content };
                return new JsonValidationResult<TModel>()
                {
                    IsValid = false,
                    Message = httpResponseMessage,
                };
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
