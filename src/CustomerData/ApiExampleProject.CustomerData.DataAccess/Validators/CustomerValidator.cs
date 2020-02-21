using ApiExampleProject.CustomerData.DataAccess.Models;
using FluentValidation;

namespace ApiExampleProject.CustomerData.DataAccess.Validators
{
    public class CustomerValidator
        : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.BirthDate).NotEmpty();
        }
    }
}
