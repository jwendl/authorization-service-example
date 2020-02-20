using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Validators
{
    public class ThingAttributeValidator
        : AbstractValidator<ThingAttribute>
    {
        public ThingAttributeValidator()
        {
            RuleFor(ta => ta.ThingId).NotEmpty();
            RuleFor(ta => ta.Key).NotEmpty();
            RuleFor(ta => ta.Value).NotEmpty();
        }
    }
}
