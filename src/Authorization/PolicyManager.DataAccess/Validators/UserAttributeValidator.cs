using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Validators
{
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
