using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly Mock<IDataRepository<User>> mockUserRepository;
        private readonly Mock<IDataRepository<Thing>> mockThingRepository;
        private readonly IServiceProvider serviceProvider;

        public AuthorizationRepositoryTests()
        {
            mockUserRepository = new Mock<IDataRepository<User>>();
            mockThingRepository = new Mock<IDataRepository<Thing>>();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton(mockUserRepository.Object);
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

            var userRepository = serviceProvider.GetRequiredService<IDataRepository<User>>();
            var thingRepository = serviceProvider.GetRequiredService<IDataRepository<Thing>>();
            var authorizationRepository = new AuthorizationRepository(userRepository, thingRepository);

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(ci => ci.Name).Returns("juswen@microsoft.com");
            mockClaimsPrincipal.Setup(cp => cp.Identity).Returns(mockClaimsIdentity.Object);

            var initialState = new InitialState<Group>()
            {
                ClaimsPrincipal = mockClaimsPrincipal.Object,
                Identifier = "/api/Customer/1",
                Groups = new List<Group>()
                {
                    new Group() { DisplayName = "Finance" },
                }
            };

            var actualResult = await authorizationRepository.EvaluateAsync(initialState);
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
                Id = Guid.NewGuid(),
            };

            mockUserRepository.Setup(ur => ur.FindSingleAndIncludeAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, ICollection<UserAttribute>>>[]>()))
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

            mockThingRepository.Setup(tr => tr.FindAsync(It.IsAny<Expression<Func<Thing, bool>>>()))
                .Returns(Task.FromResult(new List<Thing>() { expectedThing }.AsEnumerable()));

            var userRepository = serviceProvider.GetRequiredService<IDataRepository<User>>();
            var thingRepository = serviceProvider.GetRequiredService<IDataRepository<Thing>>();
            var authorizationRepository = new AuthorizationRepository(userRepository, thingRepository);

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(ci => ci.Name).Returns("juswen@microsoft.com");
            mockClaimsPrincipal.Setup(cp => cp.Identity).Returns(mockClaimsIdentity.Object);

            var initialState = new InitialState<Group>()
            {
                ClaimsPrincipal = mockClaimsPrincipal.Object,
                Groups = new List<Group>()
                {
                    new Group() { DisplayName = "Finance" },
                }
            };

            var actualResult = await authorizationRepository.EvaluateAsync(initialState);
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
                Id = Guid.NewGuid(),
            };
            expectedUser.UserAttributes.Add(new UserAttribute()
            {
                Id = Guid.NewGuid(),
                Key = "location",
                Value = "/NA/Washington/Redmond/Building 122",
            });

            mockUserRepository.Setup(ur => ur.FindSingleAndIncludeAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, ICollection<UserAttribute>>>[]>()))
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
                Expression = "user_location.Contains(\"/NA/Washington\")",
            });

            mockThingRepository.Setup(tr => tr.FindAsync(It.IsAny<Expression<Func<Thing, bool>>>()))
                .Returns(Task.FromResult(new List<Thing>() { expectedThing }.AsEnumerable()));

            var userRepository = serviceProvider.GetRequiredService<IDataRepository<User>>();
            var thingRepository = serviceProvider.GetRequiredService<IDataRepository<Thing>>();
            var authorizationRepository = new AuthorizationRepository(userRepository, thingRepository);

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(ci => ci.Name).Returns("juswen@microsoft.com");
            mockClaimsPrincipal.Setup(cp => cp.Identity).Returns(mockClaimsIdentity.Object);

            var initialState = new InitialState<Group>()
            {
                ClaimsPrincipal = mockClaimsPrincipal.Object,
                Identifier = "Robot 123",
                Groups = new List<Group>()
                {
                    new Group() { DisplayName = "Finance" },
                }
            };

            var actualResult = await authorizationRepository.EvaluateAsync(initialState);
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
                Id = Guid.NewGuid(),
            };
            expectedUser.UserAttributes.Add(new UserAttribute()
            {
                Id = Guid.NewGuid(),
                Key = "location",
                Value = "/NA/Washington/Redmond/Building 123",
            });

            mockUserRepository.Setup(ur => ur.FindSingleAndIncludeAsync(It.IsAny<Expression<Func<User, bool>>>(), It.IsAny<Expression<Func<User, ICollection<UserAttribute>>>[]>()))
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
                Expression = "user_location.Contains(\"/NA/Washington/Redmond/Building 122\")",
            });

            mockThingRepository.Setup(tr => tr.FindAsync(It.IsAny<Expression<Func<Thing, bool>>>()))
                .Returns(Task.FromResult(new List<Thing>() { expectedThing }.AsEnumerable()));

            var userRepository = serviceProvider.GetRequiredService<IDataRepository<User>>();
            var thingRepository = serviceProvider.GetRequiredService<IDataRepository<Thing>>();
            var authorizationRepository = new AuthorizationRepository(userRepository, thingRepository);

            var mockClaimsPrincipal = new Mock<ClaimsPrincipal>();
            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.Setup(ci => ci.Name).Returns("juswen@microsoft.com");
            mockClaimsPrincipal.Setup(cp => cp.Identity).Returns(mockClaimsIdentity.Object);

            var initialState = new InitialState<Group>()
            {
                ClaimsPrincipal = mockClaimsPrincipal.Object,
                Identifier = "Robot 123",
                Groups = new List<Group>()
                {
                    new Group() { DisplayName = "Finance" },
                }
            };

            var actualResult = await authorizationRepository.EvaluateAsync(initialState);
            actualResult.Should().BeEquivalentTo(expectedResult);
        }
    }
}
