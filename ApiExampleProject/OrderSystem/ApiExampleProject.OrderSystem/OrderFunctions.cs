using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Interfaces;
using ApiExampleProject.Common.Pagination;
using ApiExampleProject.OrderSystem.DataAccess.Interfaces;
using ApiExampleProject.OrderSystem.DataAccess.Models;
using ApiExampleProject.OrderSystem.DataAccess.Validators;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.OpenApi.Models;

namespace ApiExampleProject.OrderSystem
{
    public class OrderFunctions
    {
        private readonly ITokenValidator tokenValidator;
        private readonly IJsonHttpContentValidator jsonHttpContentValidator;
        private readonly IDataRepository<Order> orderRepository;

        public OrderFunctions(ITokenValidator tokenValidator, IJsonHttpContentValidator jsonHttpContentValidator, IDataRepository<Order> orderRepository)
        {
            this.tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
            this.jsonHttpContentValidator = jsonHttpContentValidator ?? throw new ArgumentNullException(nameof(jsonHttpContentValidator));
            this.orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(OrderFunctions.orderRepository));
        }

        [FunctionName(nameof(CreateOrder))]
        [OpenApiOperation(nameof(CreateOrder), "Create Order", Description = "Creates a order entry.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(Order), Description = "New customer information")]
        [OpenApiResponseBody(HttpStatusCode.Created, MediaTypeNames.Application.Json, typeof(Order))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, MediaTypeNames.Text.Plain, typeof(string))]
        public async Task<HttpResponseMessage> CreateOrder([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "orders")] HttpRequestMessage req)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(req.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var jsonValidationResult = await jsonHttpContentValidator.ValidateJsonAsync<Order, OrderValidator>(req.Content);
            if (!jsonValidationResult.IsValid)
            {
                return jsonValidationResult.Message;
            }

            var customer = await orderRepository.CreateItemAsync(jsonValidationResult.Item);
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, MediaTypeNames.Application.Json);
            return new HttpResponseMessage(HttpStatusCode.Created) { Content = content };
        }


        [FunctionName(nameof(ReadOrders))]
        [OpenApiOperation(nameof(ReadOrders), "Read Orders", Description = "Returns all orders.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(MediaTypeNames.Application.Json, typeof(PaginationRequest), Description = "Read all orders")]
        [OpenApiResponseBody(HttpStatusCode.OK, MediaTypeNames.Application.Json, typeof(IEnumerable<Order>))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, MediaTypeNames.Text.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadOrders([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "orders/{pageNumber?}/{pageSize?}")] HttpRequestMessage req, string pageNumber, string pageSize)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(req.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            if (string.IsNullOrWhiteSpace(pageNumber)) pageNumber = "0";
            if (string.IsNullOrWhiteSpace(pageSize)) pageSize = "25";
            var paginationRequest = new PaginationRequest() { PageNumber = int.Parse(pageNumber, CultureInfo.CurrentCulture), PageSize = int.Parse(pageSize, CultureInfo.CurrentCulture) };

            var customer = await orderRepository.ReadAllAsync(paginationRequest);
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, MediaTypeNames.Application.Json);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        }
    }
}
