using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Validators
{
    public class ThingPolicyValidator
        : AbstractValidator<ThingPolicy>
    {
        public ThingPolicyValidator()
        {
            RuleFor(tp => tp.ThingId).NotEmpty();
            RuleFor(tp => tp.Name).NotEmpty();
            RuleFor(tp => tp.Expression).NotEmpty();
        }
    }
}
