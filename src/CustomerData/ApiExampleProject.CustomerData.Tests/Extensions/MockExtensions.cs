using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using ApiExampleProject.Authentication.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit.Abstractions;

namespace ApiExampleProject.CustomerData.Tests.Extensions
{
    public static class MockExtensions
    {
        public static void SetupMockLogger<T>(this Mock<ILogger> mockLogger, ITestOutputHelper output)
        {
            mockLogger.Setup(log => log.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), null, It.IsAny<Func<object, Exception, string>>()))
                .Callback<LogLevel, EventId, object, Exception, Func<object, Exception, string>>((logLevel, e, state, ex, f) =>
                {
                    output.WriteLine($"{logLevel} logged: \"{state}\"");
                });
        }

        public static void SetupMockLogger<T>(this Mock<ILogger<T>> mockLogger, ITestOutputHelper output)
        {
            mockLogger.Setup(log => log.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<object>(), null, It.IsAny<Func<object, Exception, string>>()))
                .Callback<LogLevel, EventId, object, Exception, Func<object, Exception, string>>((logLevel, e, state, ex, f) =>
                {
                    output.WriteLine($"{logLevel} logged: \"{state}\"");
                });
        }

        public static void SetupMockValidClaimsPrincipal(this Mock<ITokenValidator> mockTokenValidator)
        {
            var claimsObjectId = Guid.NewGuid().ToString();
            var claimsAudienceId = Guid.NewGuid().ToString();

            var claim = new Claim("name", claimsObjectId);
            claim.Properties.Add(claimsObjectId, "oid");

            var claimAudience = new Claim("aud", claimsAudienceId);
            claimAudience.Properties.Add(claimsAudienceId, "aud");

            var claimsIdentity = new ClaimsIdentity(new List<Claim>() { claim, claimAudience });
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            mockTokenValidator.Setup(mtv => mtv.ValidateTokenAsync(It.IsAny<AuthenticationHeaderValue>()))
                .Returns(Task.FromResult(claimsPrincipal));
        }
    }
}
