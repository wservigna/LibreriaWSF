using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using WSF.Events.Bus;
using WSF.Events.Bus.Exceptions;
using WSF.Logging;
using WSF.Web.Models;

namespace WSF.WebApi.Controllers.Filters
{
    /// <summary>
    /// Used to handle exceptions on web api controllers.
    /// </summary>
    public class WSFExceptionFilterAttribute : ExceptionFilterAttribute
    {
        /// <summary>
        /// Raises the exception event.
        /// </summary>
        /// <param name="context">The context for the action.</param>
        public override void OnException(HttpActionExecutedContext context)
        {
            LogHelper.LogException(context.Exception);

            context.Response = context.Request.CreateResponse(
                HttpStatusCode.OK,
                new AjaxResponse(
                    ErrorInfoBuilder.Instance.BuildForException(context.Exception),
                    context.Exception is WSF.Authorization.WSFAuthorizationException)
                );

            EventBus.Default.Trigger(this, new WSFHandledExceptionData(context.Exception));
        }
    }
}