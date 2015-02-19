using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using WSF.Collections.Extensions;
using WSF.WebApi.Controllers.Dynamic.Builders;

namespace WSF.WebApi.Controllers.Dynamic.Selectors
{
    /// <summary>
    /// This class is used to extend default controller selector to add dynamic api controller creation feature of WSF.
    /// It checks if requested controller is a dynamic api controller, if it is,
    /// returns <see cref="HttpControllerDescriptor"/> to ASP.NET system.
    /// </summary>
    public class WSFHttpControllerSelector : DefaultHttpControllerSelector
    {
        private readonly HttpConfiguration _configuration;

        /// <summary>
        /// Creates a new <see cref="WSFHttpControllerSelector"/> object.
        /// </summary>
        /// <param name="configuration">Http configuration</param>
        public WSFHttpControllerSelector(HttpConfiguration configuration)
            : base(configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This method is called by Web API system to select the controller for this request.
        /// </summary>
        /// <param name="request">Request object</param>
        /// <returns>The controller to be used</returns>
        public override HttpControllerDescriptor SelectController(HttpRequestMessage request)
        {
            if (request != null)
            {
                var routeData = request.GetRouteData();
                if (routeData != null)
                {
                    object serviceNameWithAction;
                    if (routeData.Values.TryGetValue("serviceNameWithAction", out serviceNameWithAction) && DynamicApiServiceNameHelper.IsValidServiceNameWithAction(serviceNameWithAction.ToString()))
                    {
                        var serviceName = DynamicApiServiceNameHelper.GetServiceNameInServiceNameWithAction(serviceNameWithAction.ToString());
                        var controllerInfo = DynamicApiControllerManager.FindOrNull(serviceName);
                        if (controllerInfo != null)
                        {
                            var controllerDescriptor = new DynamicHttpControllerDescriptor(_configuration, controllerInfo.ServiceName, controllerInfo.Type, controllerInfo.Filters);
                            controllerDescriptor.Properties["__WSFDynamicApiControllerInfo"] = controllerInfo;
                            return controllerDescriptor;
                        }
                    }
                }
            }

            return base.SelectController(request);
        }
    }
}