using ApiExampleProject.Common.Pagination;
using FluentValidation;

namespace ApiExampleProject.CustomerData.DataAccess.Validators
{
    public class CustomerRequestValidator
        : AbstractValidator<PaginationRequest>
    {
        public CustomerRequestValidator()
        {

        }
    }
}
