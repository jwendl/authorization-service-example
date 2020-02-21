using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Aliencube.AzureFunctions.Extensions.OpenApi.Attributes;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Constants;
using ApiExampleProject.Common.Interfaces;
using ApiExampleProject.Common.Pagination;
using ApiExampleProject.CustomerData.DataAccess.Interfaces;
using ApiExampleProject.CustomerData.DataAccess.Models;
using ApiExampleProject.CustomerData.DataAccess.Validators;
using ApiExampleProject.CustomerData.Resources;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ApiExampleProject.CustomerData
{
    public class CustomerFunctions
    {
        private readonly ITokenValidator tokenValidator;
        private readonly IJsonHttpContentValidator jsonHttpContentValidator;
        private readonly IDataRepository<Customer> customerRepository;

        public CustomerFunctions(ITokenValidator tokenValidator, IJsonHttpContentValidator jsonHttpContentValidator, IDataRepository<Customer> customerRepository)
        {
            this.tokenValidator = tokenValidator ?? throw new ArgumentNullException(nameof(tokenValidator));
            this.jsonHttpContentValidator = jsonHttpContentValidator ?? throw new ArgumentNullException(nameof(jsonHttpContentValidator));
            this.customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
        }

        [FunctionName(nameof(CreateCustomer))]
        [OpenApiOperation(nameof(CreateCustomer), "Create Customer", Description = "Creates a customer entry.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(Customer), Description = "New customer information")]
        [OpenApiResponseBody(HttpStatusCode.Created, ContentTypes.Application.Json, typeof(Customer))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> CreateCustomer([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "customers")] HttpRequestMessage req, ILogger log)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(CustomerDataResources.CreateCustomerStartLog);

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(req.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            var jsonValidationResult = await jsonHttpContentValidator.ValidateJsonAsync<Customer, CustomerValidator>(req.Content);
            if (!jsonValidationResult.IsValid)
            {
                return jsonValidationResult.Message;
            }

            var customer = await customerRepository.CreateItemAsync(jsonValidationResult.Item);
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, ContentTypes.Application.Json);

            log.LogInformation(CustomerDataResources.CreateCustomerEndLog);
            return new HttpResponseMessage(HttpStatusCode.Created) { Content = content };
        }


        [FunctionName(nameof(ReadCustomers))]
        [OpenApiOperation(nameof(ReadCustomers), "Read Customers", Description = "Returns all customers.")]
        [OpenApiParameter("Authorization", In = ParameterLocation.Header, Required = true, Type = typeof(string))]
        [OpenApiRequestBody(ContentTypes.Application.Json, typeof(PaginationRequest), Description = "Read all customers")]
        [OpenApiResponseBody(HttpStatusCode.OK, ContentTypes.Application.Json, typeof(IEnumerable<Customer>))]
        [OpenApiResponseBody(HttpStatusCode.BadRequest, ContentTypes.TextType.Plain, typeof(string))]
        public async Task<HttpResponseMessage> ReadCustomers([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "customers/{pageNumber?}/{pageSize?}")] HttpRequestMessage req, ILogger log, string pageNumber, string pageSize)
        {
            _ = req ?? throw new ArgumentNullException(nameof(req));

            log.LogInformation(CustomerDataResources.ReadCustomersStartLog);

            var claimsPrincipal = await tokenValidator.ValidateTokenAsync(req.Headers.Authorization);
            if (claimsPrincipal == null) return new HttpResponseMessage(HttpStatusCode.Unauthorized);

            if (string.IsNullOrWhiteSpace(pageNumber)) pageNumber = "0";
            if (string.IsNullOrWhiteSpace(pageSize)) pageSize = "25";
            var paginationRequest = new PaginationRequest() { PageNumber = int.Parse(pageNumber, CultureInfo.CurrentCulture), PageSize = int.Parse(pageSize, CultureInfo.CurrentCulture) };

            var customer = await customerRepository.ReadAllAsync(paginationRequest);
            var content = new StringContent(JsonSerializer.Serialize(customer), Encoding.UTF8, ContentTypes.Application.Json);

            log.LogInformation(CustomerDataResources.ReadCustomerEndLog);
            return new HttpResponseMessage(HttpStatusCode.OK) { Content = content };
        }
    }
}
