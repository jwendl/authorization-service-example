using FluentValidation;
using PolicyManager.DataAccess.Models;

namespace PolicyManager.Validators
{
    public class CheckAccessRequestValidator
    : AbstractValidator<CheckAccessRequest>
    {
        public CheckAccessRequestValidator()
        {

        }
    }
}
