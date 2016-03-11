using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using Contracts.Responses;

namespace Canvas001.Attributes
{
    public class ValidationAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutingAsync(HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            var modelState = actionContext.ModelState;
            if (!modelState.IsValid)
            {
                var data = modelState["ValidationReport"].Errors[0].ErrorMessage;

                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, string.Empty);
                actionContext.Response.Content = new StringContent(data, Encoding.UTF8, "application/json");
            }
            return base.OnActionExecutingAsync(actionContext, cancellationToken);
        }

        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            base.OnActionExecuted(actionExecutedContext);
        }

        public override Task OnActionExecutedAsync(HttpActionExecutedContext actionExecutedContext, CancellationToken cancellationToken)
        {
            var objectContent = (ObjectContent)actionExecutedContext.ActionContext.Response.Content;
            var responseType = objectContent.ObjectType;
            if (typeof (ResponseBase).IsAssignableFrom(responseType))
            {
                var result = objectContent.Value as ResponseBase;
                if (result != null && (result.Error == null || result.Validation == null))
                {
                    actionExecutedContext.Response = 
                        actionExecutedContext.Request.CreateResponse(HttpStatusCode.Forbidden, result.Validation);
                }
            }

            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}