using Contracts;
using FluentValidation;

namespace Worker.Validators
{

    public class CurrencyRequestValidator : AbstractValidator<CurrencyRequest>
    {
        public CurrencyRequestValidator()
        {
            RuleFor(p => p)
                .Must(p =>
                {
                    return true; //false;
                });
        }
    }
}
