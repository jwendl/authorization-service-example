using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Validators
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is not a collection class.")]
    public class UserAttributeValidator
        : AbstractValidator<UserAttribute>
    {
        public UserAttributeValidator()
        {
            RuleFor(ua => ua.UserId).NotEmpty();
            RuleFor(ua => ua.Key).NotEmpty();
            RuleFor(ua => ua.Value).NotEmpty();
        }
    }
}
