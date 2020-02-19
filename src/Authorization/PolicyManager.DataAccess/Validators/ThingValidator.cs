using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Validators
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is not a collection class.")]
    public class ThingValidator
        : AbstractValidator<Thing>
    {
        public ThingValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
        }
    }
}
