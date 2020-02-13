using System.Diagnostics.CodeAnalysis;
using ApiExampleProject.Common.Pagination;
using FluentValidation;

namespace ApiExampleProject.OrderSystem.DataAccess.Validators
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is not a collection class.")]
    public class OrderRequestValidator
        : AbstractValidator<PaginationRequest>
    {
        public OrderRequestValidator()
        {

        }
    }
}
