using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Validators
{
    public class ThingValidator
        : AbstractValidator<Thing>
    {
        public ThingValidator()
        {
            RuleFor(r => r.Name).NotEmpty();
        }
    }
}
