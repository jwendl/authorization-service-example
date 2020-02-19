using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Validators
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is not a collection class.")]
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
