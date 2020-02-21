using System.Net.Http;
using System.Threading.Tasks;
using ApiExampleProject.Common.Models;
using FluentValidation;

namespace ApiExampleProject.Common.Interfaces
{
    public interface IJsonHttpContentValidator
    {
        Task<JsonValidationResult<TModel>> ValidateJsonAsync<TModel, TValidator>(HttpContent httpContent)
            where TValidator : AbstractValidator<TModel>, new();
    }
}
