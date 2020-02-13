using System.Diagnostics.CodeAnalysis;
using ApiExampleProject.OrderSystem.DataAccess.Models;
using FluentValidation;

namespace ApiExampleProject.OrderSystem.DataAccess.Validators
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is not a collection class.")]
    public class OrderValidator
        : AbstractValidator<Order>
    {
        public OrderValidator()
        {
            RuleFor(o => o.OrderDate).NotEmpty();
        }
    }
}
