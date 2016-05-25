using System.Linq;
using Contracts.Responses;
using FluentValidation.Results;

namespace Worker
{
    public static class FluentValidationExtensions
    {
        public static Error AsError(this ValidationResult validation)
        {
            return new Error
            {
                ErrorCode = validation.Errors.First().ErrorCode,
                ErrorMessage = validation.Errors.First().ErrorMessage,
                Errors = validation.Errors.Select(v => new ErrorInfo
                {
                    ErrorCode = v.ErrorCode,
                    ErrorMessage = v.ErrorMessage,
                    PropertyName = v.PropertyName
                }).ToList()
            };
        }
    }
}