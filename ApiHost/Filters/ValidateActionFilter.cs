using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Autofac;
using Autofac.Integration.WebApi;
using Contracts.Responses;
using FluentValidation;
using FluentValidation.Results;

namespace Canvas001.Attributes
{
    public class ValidateActionFilter : IAutofacActionFilter
    {
        private readonly ILifetimeScope _lifetimeScope;

        public ValidateActionFilter(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public void OnActionExecuting(HttpActionContext actionContext)
        {
            foreach (var item in actionContext.ActionArguments.Values)
            {
                var vResult = Validate(item);
                if (vResult != null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, vResult);
                    return;
                }
            }
        }

        public void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var content = (ObjectContent)actionExecutedContext.ActionContext.Response.Content;
            var result = content?.Value as ResponseBase;
            if (result?.Validation != null)
            {
                actionExecutedContext.Response =
                        actionExecutedContext.Request.CreateResponse(HttpStatusCode.Forbidden, result.Validation);
                return;
            }
            if (result?.Error != null)
            {
                actionExecutedContext.Response =
                    actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, result.Error);
            }
        }

        public Error Validate(object item)
        {
            var validator = GetValidatorFor(item);
            var vResult = validator?.Validate(item);
            return vResult?.IsValid == false
                ? vResult.AsError()
                : null;
        }

        public IValidator GetValidatorFor(object item)
        {
            var typeInRuntime = item.GetType();
            var validatorType = typeof(AbstractValidator<>).MakeGenericType(typeInRuntime);
            return _lifetimeScope.IsRegistered(validatorType)
                ? _lifetimeScope.Resolve(validatorType) as IValidator
                : null;
        }
    }

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