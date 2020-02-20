using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.DataAccess.Validators
{
    public class UserValidator
        : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(c => c.UserPrincipalName).NotEmpty();
        }
    }
}
