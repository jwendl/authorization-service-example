using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ApiExampleProject.Authentication.Interfaces;
using ApiExampleProject.Common.Constants;
using ApiExampleProject.Common.Interfaces;
using ApiExampleProject.Common.Models;
using ApiExampleProject.Common.Pagination;
using ApiExampleProject.CustomerData.DataAccess.Interfaces;
using ApiExampleProject.CustomerData.DataAccess.Models;
using ApiExampleProject.CustomerData.DataAccess.Validators;
using ApiExampleProject.CustomerData.Tests.Extensions;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace ApiExampleProject.CustomerData.Tests
{
    public class CustomerFunctionsTests
    {
        private readonly Mock<ITokenValidator> mockTokenValidator;
        private readonly Mock<IJsonHttpContentValidator> mockJsonHttpContentValidator;
        private readonly Mock<IDataRepository<Customer>> mockCustomerRepository;
        private readonly Mock<ILogger> mockLogger;
        private readonly IServiceProvider serviceProvider;

        public CustomerFunctionsTests(ITestOutputHelper testOutputHelper)
        {
            mockTokenValidator = new Mock<ITokenValidator>();
            mockTokenValidator.SetupMockValidClaimsPrincipal();

            mockJsonHttpContentValidator = new Mock<IJsonHttpContentValidator>();
            mockCustomerRepository = new Mock<IDataRepository<Customer>>();
            
            mockLogger = new Mock<ILogger>();
            mockLogger.SetupMockLogger<ILogger>(testOutputHelper);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(mockTokenValidator.Object);
            serviceCollection.AddSingleton(mockJsonHttpContentValidator.Object);
            serviceCollection.AddSingleton(mockCustomerRepository.Object);
            serviceCollection.AddSingleton<CustomerFunctions>();
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public void CustomerFunctions_ConstructorWhenRequirementsAreNull_ThrowsException()
        {
            Func<CustomerFunctions> customerAction = () => new CustomerFunctions(null, mockJsonHttpContentValidator.Object, mockCustomerRepository.Object);
            customerAction.Should().Throw<ArgumentNullException>();

            customerAction = () => new CustomerFunctions(mockTokenValidator.Object, null, mockCustomerRepository.Object);
            customerAction.Should().Throw<ArgumentNullException>();

            customerAction = () => new CustomerFunctions(mockTokenValidator.Object, mockJsonHttpContentValidator.Object, null);
            customerAction.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public async Task CreateCustomer_WhenCalled_ReturnsValidCustomer()
        {
            var expectedCustomer = new Customer()
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = DateTime.Now.AddYears(-18),
            };

            mockCustomerRepository.Setup(cr => cr.CreateItemAsync(expectedCustomer))
                .Returns(Task.FromResult(new Customer()
                {
                    Id = Guid.NewGuid(),
                    FirstName = expectedCustomer.FirstName,
                    LastName = expectedCustomer.LastName,
                    BirthDate = expectedCustomer.BirthDate,
                }));

            var jsonValidationResult = new JsonValidationResult<Customer>()
            {
                IsValid = true,
                Item = expectedCustomer,
            };
            mockJsonHttpContentValidator.Setup(jhcv => jhcv.ValidateJsonAsync<Customer, CustomerValidator>(It.IsAny<StringContent>()))
                .Returns(Task.FromResult(jsonValidationResult));

            using var httpRequestMessage = new HttpRequestMessage()
            {
                Content = new StringContent(JsonSerializer.Serialize(expectedCustomer), Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            var customerFunctions = serviceProvider.GetRequiredService<CustomerFunctions>();
            var httpResponseMessage = await customerFunctions.CreateCustomer(httpRequestMessage, mockLogger.Object);
            httpResponseMessage.IsSuccessStatusCode.Should().BeTrue();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.Created);
            httpResponseMessage.Content.Headers.ContentType.MediaType.Should().Be(ContentTypes.Application.Json);

            var actualCustomer = await httpResponseMessage.Content.ReadAsAsync<Customer>();
            actualCustomer.Id.Should().NotBeEmpty();
            actualCustomer.FirstName.Should().Be(expectedCustomer.FirstName);
            actualCustomer.LastName.Should().Be(expectedCustomer.LastName);
            actualCustomer.BirthDate.Should().Be(expectedCustomer.BirthDate);
        }

        [Fact]
        public async Task CreateCustomer_WhenCalledWithInvalidJson_ReturnsBadRequest()
        {
            var expectedCustomer = default(Customer);
            mockCustomerRepository.Setup(cr => cr.CreateItemAsync(expectedCustomer))
                .Returns(Task.FromResult(new Customer()));

            var jsonValidationResult = new JsonValidationResult<Customer>()
            {
                IsValid = false,
                Message = new HttpResponseMessage(HttpStatusCode.BadRequest),
            };
            mockJsonHttpContentValidator.Setup(jhcv => jhcv.ValidateJsonAsync<Customer, CustomerValidator>(It.IsAny<StringContent>()))
                .Returns(Task.FromResult(jsonValidationResult));

            using var httpRequestMessage = new HttpRequestMessage()
            {
                Content = new StringContent(JsonSerializer.Serialize(expectedCustomer), Encoding.UTF8, MediaTypeNames.Application.Json)
            };

            var customerFunctions = serviceProvider.GetRequiredService<CustomerFunctions>();
            var httpResponseMessage = await customerFunctions.CreateCustomer(httpRequestMessage, mockLogger.Object);
            httpResponseMessage.IsSuccessStatusCode.Should().BeFalse();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ReadCustomer_WhenCalled_ReturnsCustomers()
        {
            var expectedCustomers = new List<Customer>()
            {
                new Customer()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "Jane",
                    LastName = "Doe",
                    BirthDate = DateTime.UtcNow.AddYears(-18),
                },
                new Customer()
                {
                    Id = Guid.NewGuid(),
                    FirstName = "John",
                    LastName = "Doe",
                    BirthDate = DateTime.UtcNow.AddYears(-21),
                }
            };

            mockCustomerRepository.Setup(cr => cr.ReadAllAsync(It.IsAny<PaginationRequest>()))
                .Returns(Task.FromResult(expectedCustomers.AsEnumerable()));

            using var httpRequestMessage = new HttpRequestMessage();
            var customerFunctions = serviceProvider.GetRequiredService<CustomerFunctions>();
            var httpResponseMessage = await customerFunctions.ReadCustomers(httpRequestMessage, mockLogger.Object, null, null);
            httpResponseMessage.IsSuccessStatusCode.Should().BeTrue();
            httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
            httpResponseMessage.Content.Headers.ContentType.MediaType.Should().Be(ContentTypes.Application.Json);

            var actualCustomers = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<Customer>>();
            actualCustomers.Count().Should().Be(2);
        }
    }
}
