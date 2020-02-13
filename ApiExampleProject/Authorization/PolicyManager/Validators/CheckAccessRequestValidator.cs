using System.Diagnostics.CodeAnalysis;
using FluentValidation;
using PolicyManager.Models;

namespace PolicyManager.Validators
{
    [SuppressMessage("Naming", "CA1710:Identifiers should have correct suffix", Justification = "This is not a collection class.")]
    public class CheckAccessRequestValidator
    : AbstractValidator<CheckAccessRequest>
    {
        public CheckAccessRequestValidator()
        {

        }
    }
}
