using Contracts.Requests;
using Contracts.Responses;
using FluentValidation;
using MassTransit;

namespace Canvas001.Validators
{
    public class ComplexRequestValidator : AbstractValidator<ComplexRequest>
    {
        public ComplexRequestValidator(IRequestClient<CurrencyRequest, CurrencyResponse> client)
        {
            RuleFor(p => p.Name)
                .Must(p =>
                {
                    var data = client.Request(new CurrencyRequest());
                    var x = data.Result.Currencies;
                    return true;
                });

            RuleFor(p => p.Amount).Must(p => p > 1000).WithErrorCode("XXX").WithMessage("YYYY");
            RuleFor(p => p.Brand).NotEmpty();
            RuleFor(p => p.Name).NotEmpty();
        }
    }
}