using System.Linq;
using FluentValidation;
using FluentValidation.Results;
using Newtonsoft.Json;

namespace Canvas001.Validators
{
    public class BaseValidator<T> : AbstractValidator<T>
    {
        public override ValidationResult Validate(ValidationContext<T> context)
        {
            var vResult = base.Validate(context);
            if (vResult.IsValid)
            {
                return vResult;
            }

            var results = new
            {
                vResult.Errors.First().ErrorCode,
                vResult.Errors.First().ErrorMessage,
                Errors = vResult.Errors.Select(v => new
                {
                    v.ErrorCode,
                    v.ErrorMessage,
                    v.PropertyName
                })
            };
            var error = JsonConvert.SerializeObject(results, Formatting.Indented);

            vResult.Errors.Clear();
            vResult.Errors.Insert(0, new ValidationFailure("ValidationReport", error));
            return vResult;
        }
    }
}