using System;
using System.Web;
using System.Web.Mvc;
using WSF.Events.Bus;
using WSF.Events.Bus.Exceptions;
using WSF.Logging;
using WSF.Web.Models;
using WSF.Web.Mvc.Controllers.Results;
using WSF.Web.Mvc.Models;

namespace WSF.Web.Mvc.Controllers
{
    /// <summary>
    /// Used internally by WSF to handle exceptions on MVC actions.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
    public class WSFHandleErrorAttribute : HandleErrorAttribute /* This class is written by looking at the source codes of System.Web.Mvc.HandleErrorAttribute class */
    {
        /// <summary>
        /// Called when an exception occurs.
        /// </summary>
        /// <param name="context">The exception context.</param>
        public override void OnException(ExceptionContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            //If exception handled before, do nothing.
            //If this is child action, exception should be handled by main action.
            if (context.ExceptionHandled || context.IsChildAction)
            {
                return;
            }

            //Always log exception
            LogHelper.LogException(context.Exception);

            // If custom errors are disabled, we need to let the normal ASP.NET exception handler
            // execute so that the user can see useful debugging information.
            if (!context.HttpContext.IsCustomErrorEnabled)
            {
                return;
            }

            // If this is not an HTTP 500 (for example, if somebody throws an HTTP 404 from an action method),
            // ignore it.
            if (new HttpException(null, context.Exception).GetHttpCode() != 500)
            {
                return;
            }

            //Do not handle exceptions for attributes configured for special exception types and this exceptiod does not fit condition.
            if (!ExceptionType.IsInstanceOfType(context.Exception))
            {
                return;
            }

            //We handled the exception!
            context.ExceptionHandled = true;

            //Return a special error response to the client.
            context.HttpContext.Response.Clear();
            context.Result = IsAjaxRequest(context)
                ? GenerateAjaxResult(context)
                : GenerateNonAjaxResult(context);

            // Certain versions of IIS will sometimes use their own error page when
            // they detect a server error. Setting this property indicates that we
            // want it to try to render ASP.NET MVC's error page instead.
            context.HttpContext.Response.TrySkipIisCustomErrors = true;

            //Trigger an event, so we can register it.
            EventBus.Default.Trigger(this, new WSFHandledExceptionData(context.Exception));
        }

        private static bool IsAjaxRequest(ExceptionContext context)
        {
            return context.HttpContext.Request.IsAjaxRequest();
        }

        private static ActionResult GenerateAjaxResult(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = 200;
            return new WSFJsonResult(
                new MvcAjaxResponse(
                    ErrorInfoBuilder.Instance.BuildForException(context.Exception),
                    context.Exception is WSF.Authorization.WSFAuthorizationException
                    )
                );
        }

        private ActionResult GenerateNonAjaxResult(ExceptionContext context)
        {
            context.HttpContext.Response.StatusCode = 500;
            return new ViewResult
                   {
                       ViewName = View,
                       MasterName = Master,
                       ViewData = new ViewDataDictionary<ErrorViewModel>(new ErrorViewModel(context.Exception)),
                       TempData = context.Controller.TempData
                   };
        }
    }
}