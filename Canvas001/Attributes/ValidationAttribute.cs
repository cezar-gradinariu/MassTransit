using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.UI;
using Autofac;
using Autofac.Integration.WebApi;
using Contracts.Responses;
using FluentValidation;
using Newtonsoft.Json;

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
                var typeInRuntime = item.GetType();
                var validator = _lifetimeScope.Resolve(typeof (AbstractValidator<>)
                                              .MakeGenericType(typeInRuntime)) as FluentValidation.IValidator;

                if (validator == null)
                {
                    continue;
                }

                var vResult = validator.Validate(item);
                if (vResult.IsValid)
                {
                    continue;
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

                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Forbidden, results);
            }
        }

        public void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var objectContent = (ObjectContent)actionExecutedContext.ActionContext.Response.Content;
            var responseType = objectContent.ObjectType;
            if (typeof (ResponseBase).IsAssignableFrom(responseType))
            {
                var result = objectContent.Value as ResponseBase;
                if (result != null)
                {
                    if (result.Validation != null)
                    {
                        actionExecutedContext.Response =
                            actionExecutedContext.Request.CreateResponse(HttpStatusCode.Forbidden, result.Validation);
                    }
                    else
                    {
                        if (result.Error != null)
                        {
                            actionExecutedContext.Response =
                                actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, result.Error);
                        }
                    }
                }
            }
        }
    }


    public class ValidationAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            //var modelState = actionContext.ModelState;
            //if (!modelState.IsValid)
            //{
            //    var data = modelState["ValidationReport"].Errors[0].ErrorMessage;

            //    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, string.Empty);
            //    actionContext.Response.Content = new StringContent(data, Encoding.UTF8, "application/json");
            //}
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            //var objectContent = (ObjectContent)actionExecutedContext.ActionContext.Response.Content;
            //var responseType = objectContent.ObjectType;
            //if (typeof(ResponseBase).IsAssignableFrom(responseType))
            //{
            //    var result = objectContent.Value as ResponseBase;
            //    if (result != null)
            //    {
            //        if (result.Validation != null)
            //        {
            //            actionExecutedContext.Response =
            //                actionExecutedContext.Request.CreateResponse(HttpStatusCode.Forbidden, result.Validation);
            //        }
            //        else
            //        {
            //            if (result.Error != null)
            //            {
            //                actionExecutedContext.Response =
            //                    actionExecutedContext.Request.CreateResponse(HttpStatusCode.InternalServerError, result.Error);
            //            }
            //        }
            //    }
            //}
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}