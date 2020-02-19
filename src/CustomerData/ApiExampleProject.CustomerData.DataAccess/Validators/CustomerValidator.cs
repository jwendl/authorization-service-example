using System.Diagnostics.CodeAnalysis;
using ApiExampleProject.CustomerData.DataAccess.Models;
using FluentValidation;

namespace ApiExampleProject.CustomerData.DataAccess.Validators
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is not a collection class.")]
    public class CustomerValidator
        : AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.BirthDate).NotEmpty();
        }
    }
}
