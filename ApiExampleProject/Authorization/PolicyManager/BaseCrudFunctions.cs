using System;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Constants;
using ApiExampleProject.Common.Interfaces;
using ApiExampleProject.Common.Pagination;
using FluentValidation;
using PolicyManager.DataAccess.Interfaces;
using PolicyManager.DataAccess.Models;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace PolicyManager
{
    public abstract class BaseCrudFunctions<T>
        where T : BaseDatabaseModel
    {
        private readonly ITokenValidator tokenValidator;
        private readonly IJsonHttpContentValidator jsonHttpContentValidator;
        private readonly IDataRepository<T> dataRepository;

        protected BaseCrudFunctions(ITokenValidator tokenValidator, IJsonHttpContentValidator jsonHttpContentValidator, IDataRepository<T> dataRepository)
        {
            this.tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
            this.jsonHttpContentValidator = jsonHttpContentValidator ?? throw new ArgumentNullException(nameof(jsonHttpContentValidator));
            this.dataRepository = dataRepository ?? throw new ArgumentNullException(nameof(dataRepository));
        }

        protected async Task<HttpResponseMessage> CreateItemAsync<TValidator>(HttpRequestMessage httpRequestMessage)
            where TValidator : AbstractValidator<T>, new()
        {
            _ = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(httpRequestMessage.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var jsonValidationResult = await jsonHttpContentValidator.ValidateJsonAsync<T, TValidator>(httpRequestMessage.Content);
            if (!jsonValidationResult.IsValid)
            {
                return jsonValidationResult.Message;
            }

            var createdItem = await dataRepository.CreateItemAsync(jsonValidationResult.Item);
            var content = new StringContent(JsonSerializer.Serialize(createdItem), Encoding.UTF8, ContentTypes.Application.Json);
            return new HttpResponseMessage(HttpStatusCode.Created) { Content = content };
        }

        protected async Task<HttpResponseMessage> ReadItemsAsync(HttpRequestMessage httpRequestMessage, string pageNumber, string pageSize)
        {
            _ = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(httpRequestMessage.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            if (string.IsNullOrWhiteSpace(pageNumber)) pageNumber = "0";
            if (string.IsNullOrWhiteSpace(pageSize)) pageSize = "25";
            var paginationRequest = new PaginationRequest() { PageNumber = int.Parse(pageNumber, CultureInfo.CurrentCulture), PageSize = int.Parse(pageSize, CultureInfo.CurrentCulture) };

            var items = await dataRepository.ReadAllAsync(paginationRequest);
            var content = new StringContent(JsonSerializer.Serialize(items), Encoding.UTF8, ContentTypes.Application.Json);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        }

        protected async Task<HttpResponseMessage> ReadItemAsync(HttpRequestMessage httpRequestMessage, Guid id)
        {
            _ = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(httpRequestMessage.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var item = await dataRepository.ReadItemAsync(id);
            var content = new StringContent(JsonSerializer.Serialize(item), Encoding.UTF8, ContentTypes.Application.Json);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        }

        protected async Task<HttpResponseMessage> UpdateItemAsync<TValidator>(HttpRequestMessage httpRequestMessage, string id)
            where TValidator : AbstractValidator<T>, new()
        {
            _ = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(httpRequestMessage.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var item = await httpRequestMessage.Content.ReadAsAsync<T>();
            if (Guid.Parse(id) != item.Id) return new HttpResponseMessage(HttpStatusCode.BadRequest);

            var jsonValidationResult = await jsonHttpContentValidator.ValidateJsonAsync<T, TValidator>(httpRequestMessage.Content);
            if (!jsonValidationResult.IsValid)
            {
                return jsonValidationResult.Message;
            }

            var updatedItem = await dataRepository.UpdateItemAsync(item);
            var content = new StringContent(JsonSerializer.Serialize(updatedItem), Encoding.UTF8, ContentTypes.Application.Json);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        }

        protected async Task<HttpResponseMessage> DeleteItemAsync(HttpRequestMessage httpRequestMessage, string id)
        {
            _ = httpRequestMessage ?? throw new ArgumentNullException(nameof(httpRequestMessage));

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(httpRequestMessage.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            await dataRepository.DeleteItemAsync(Guid.Parse(id));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
