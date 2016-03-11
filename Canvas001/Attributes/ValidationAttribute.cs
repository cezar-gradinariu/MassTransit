using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

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
            //var x = System.Runtime.Remoting.Messaging.CallContext.LogicalGetData("ID");
            return base.OnActionExecutedAsync(actionExecutedContext, cancellationToken);
        }
    }
}