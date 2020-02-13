using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiExampleProject.Authentication.Interfaces
{
    public interface ITokenValidator
    {
        Task<ClaimsPrincipal> ValidateTokenAsync(AuthenticationHeaderValue authenticationHeaderValue);

        Task<ClaimsPrincipal> ValidateTokenAsync(string token);
    }
}
