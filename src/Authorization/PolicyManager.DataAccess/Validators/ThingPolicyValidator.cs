using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Validators
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is not a collection class.")]
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
