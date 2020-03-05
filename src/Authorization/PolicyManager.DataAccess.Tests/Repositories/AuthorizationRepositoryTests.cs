using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Graph;
using Moq;
using PolicyManager.DataAccess.Interfaces;
using PolicyManager.DataAccess.Models;
using PolicyManager.DataAccess.Repositories;
using Xunit;
using Group = Microsoft.Graph.Group;

namespace PolicyManager.DataAccess.Tests.Repositories
{
    public class AuthorizationRepositoryTests
    {
        private readonly Mock<IMicrosoftGraphRepository> mockMicrosoftGraphRepository;
        private readonly Mock<IDataRepository<Thing>> mockThingRepository;
        private readonly IServiceProvider serviceProvider;

        public AuthorizationRepositoryTests()
        {
            mockMicrosoftGraphRepository = new Mock<IMicrosoftGraphRepository>();
            mockThingRepository = new Mock<IDataRepository<Thing>>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(mockMicrosoftGraphRepository.Object);
            serviceCollection.AddSingleton(mockThingRepository.Object);
            serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task CanEvaluateAsync_NormalCase()
        {
            var expectedResult = new List<PolicyResult>()
            {
                new PolicyResult()
                {
                    Name = "None",
                    Description = "The default deny policy",
                    Result = PolicyEvaluation.Deny,
                }
            };

            var microsoftGraphRepository = serviceProvider.GetRequiredService<IMicrosoftGraphRepository>();
            var thingRepository = serviceProvider.GetRequiredService<IDataRepository<Thing>>();
            var authorizationRepository = new AuthorizationRepository(microsoftGraphRepository, thingRepository);

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(ci => ci.Name).Returns("juswen@microsoft.com");
            mockClaimsPrincipal.Setup(cp => cp.Identity).Returns(mockClaimsIdentity.Object);

            var authenticationHeaderValue = new AuthenticationHeaderValue("test");
            var initialState = new InitialState<Group>()
            {
                ClaimsPrincipal = mockClaimsPrincipal.Object,
                Identifier = "/api/Customer/1",
                Groups = new List<Group>()
                {
                    new Group() { DisplayName = "Finance" },
                }
            };

            var actualResult = await authorizationRepository.EvaluateAsync(authenticationHeaderValue, initialState);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CanEvaluateAsync_WithEnvironmentPolicies()
        {
            var expectedResult = new List<PolicyResult>()
            {
                new PolicyResult()
                {
                    Name = "Test Environment Policy",
                    Result = PolicyEvaluation.Allow,
                }
            };

            var expectedUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
            };

            mockMicrosoftGraphRepository.Setup(ur => ur.FetchMeAsync(It.IsAny<AuthenticationHeaderValue>()))
                .Returns(Task.FromResult(expectedUser));

            var expectedThing = new Thing()
            {
                Id = Guid.NewGuid(),
            };
            expectedThing.ThingPolicies.Add(new ThingPolicy()
            {
                Id = Guid.NewGuid(),
                Name = "Test Environment Policy",
                Expression = "userPrincipalName = \"juswen@microsoft.com\"",
            });

            mockThingRepository.Setup(tr => tr.FindAsync(It.IsAny<Expression<Func<Thing, bool>>>(), It.IsAny<Expression<Func<Thing, object>>[]>()))
                .Returns(Task.FromResult(new List<Thing>() { expectedThing }.AsEnumerable()));

            var microsoftGraphRepository = serviceProvider.GetRequiredService<IMicrosoftGraphRepository>();
            var thingRepository = serviceProvider.GetRequiredService<IDataRepository<Thing>>();
            var authorizationRepository = new AuthorizationRepository(microsoftGraphRepository, thingRepository);

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(ci => ci.Name).Returns("juswen@microsoft.com");
            mockClaimsPrincipal.Setup(cp => cp.Identity).Returns(mockClaimsIdentity.Object);

            var authenticationHeaderValue = new AuthenticationHeaderValue("test");
            var initialState = new InitialState<Group>()
            {
                ClaimsPrincipal = mockClaimsPrincipal.Object,
                Groups = new List<Group>()
                {
                    new Group() { DisplayName = "Finance" },
                }
            };

            var actualResult = await authorizationRepository.EvaluateAsync(authenticationHeaderValue, initialState);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CanEvaluateAsync_WithLocationAttributes()
        {
            var expectedResult = new List<PolicyResult>()
            {
                new PolicyResult()
                {
                    Name = "Washington is Valid",
                    Result = PolicyEvaluation.Allow,
                }
            };

            var expectedUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
                OfficeLocation = "Carnation, WA",
            };

            mockMicrosoftGraphRepository.Setup(ur => ur.FetchMeAsync(It.IsAny<AuthenticationHeaderValue>()))
                .Returns(Task.FromResult(expectedUser));

            var expectedThing = new Thing()
            {
                Id = Guid.NewGuid(),
                Identifier = "Robot 123",
            };
            expectedThing.ThingPolicies.Add(new ThingPolicy()
            {
                Id = Guid.NewGuid(),
                Name = "Washington is Valid",
                Expression = "user_location.Contains(\"Carnation, WA\")",
            });

            mockThingRepository.Setup(tr => tr.FindAsync(It.IsAny<Expression<Func<Thing, bool>>>(), It.IsAny<Expression<Func<Thing, object>>[]>()))
                .Returns(Task.FromResult(new List<Thing>() { expectedThing }.AsEnumerable()));

            var microsoftGraphRepository = serviceProvider.GetRequiredService<IMicrosoftGraphRepository>();
            var thingRepository = serviceProvider.GetRequiredService<IDataRepository<Thing>>();
            var authorizationRepository = new AuthorizationRepository(microsoftGraphRepository, thingRepository);

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(ci => ci.Name).Returns("juswen@microsoft.com");
            mockClaimsPrincipal.Setup(cp => cp.Identity).Returns(mockClaimsIdentity.Object);

            var authenticationHeaderValue = new AuthenticationHeaderValue("test");
            var initialState = new InitialState<Group>()
            {
                ClaimsPrincipal = mockClaimsPrincipal.Object,
                Identifier = "Robot 123",
                Groups = new List<Group>()
                {
                    new Group() { DisplayName = "Finance" },
                }
            };

            var actualResult = await authorizationRepository.EvaluateAsync(authenticationHeaderValue, initialState);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public async Task CanEvaluateAsync_WithLocationAttributesReturningDeny()
        {
            var expectedResult = new List<PolicyResult>()
            {
                new PolicyResult()
                {
                    Name = "Washington is Valid",
                    Result = PolicyEvaluation.Deny,
                }
            };

            var expectedUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
                OfficeLocation = "Carnation, WA",
            };

            mockMicrosoftGraphRepository.Setup(ur => ur.FetchMeAsync(It.IsAny<AuthenticationHeaderValue>()))
                .Returns(Task.FromResult(expectedUser));

            var expectedThing = new Thing()
            {
                Id = Guid.NewGuid(),
                Identifier = "Robot 123",
            };
            expectedThing.ThingPolicies.Add(new ThingPolicy()
            {
                Id = Guid.NewGuid(),
                Name = "Washington is Valid",
                Expression = "user_location.Contains(\"Redmond, WA\")",
            });

            mockThingRepository.Setup(tr => tr.FindAsync(It.IsAny<Expression<Func<Thing, bool>>>(), It.IsAny<Expression<Func<Thing, object>>[]>()))
                .Returns(Task.FromResult(new List<Thing>() { expectedThing }.AsEnumerable()));

            var microsoftGraphRepository = serviceProvider.GetRequiredService<IMicrosoftGraphRepository>();
            var thingRepository = serviceProvider.GetRequiredService<IDataRepository<Thing>>();
            var authorizationRepository = new AuthorizationRepository(microsoftGraphRepository, thingRepository);

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(ci => ci.Name).Returns("juswen@microsoft.com");
            mockClaimsPrincipal.Setup(cp => cp.Identity).Returns(mockClaimsIdentity.Object);

            var authenticationHeaderValue = new AuthenticationHeaderValue("test"); 
            var initialState = new InitialState<Group>()
            {
                ClaimsPrincipal = mockClaimsPrincipal.Object,
                Identifier = "Robot 123",
                Groups = new List<Group>()
                {
                    new Group() { DisplayName = "Finance" },
                }
            };

            var actualResult = await authorizationRepository.EvaluateAsync(authenticationHeaderValue, initialState);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
