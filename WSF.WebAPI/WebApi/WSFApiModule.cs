using System.Net.Http.Formatting;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using WSF.Modules;
using WSF.Web;
using WSF.WebApi.Controllers;
using WSF.WebApi.Controllers.Dynamic;
using WSF.WebApi.Controllers.Dynamic.Formatters;
using WSF.WebApi.Controllers.Dynamic.Selectors;
using WSF.WebApi.Controllers.Filters;
using Newtonsoft.Json.Serialization;

namespace WSF.WebApi
{
    /// <summary>
    /// This module provides WSF features for ASP.NET Web API.
    /// </summary>
    [DependsOn(typeof(WSFWebModule))]
    public class WSFWebApiModule : WSFModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ApiControllerConventionalRegistrar());
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            InitializeAspNetServices();
            InitializeFilters();
            InitializeFormatters();
            InitializeRoutes();
        }

        private static void InitializeAspNetServices()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector), new WSFHttpControllerSelector(GlobalConfiguration.Configuration));
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpActionSelector), new WSFApiControllerActionSelector());
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WSFControllerActivator());
        }

        private static void InitializeFilters()
        {
            GlobalConfiguration.Configuration.Filters.Add(new WSFExceptionFilterAttribute());
        }

        private static void InitializeFormatters()
        {
            GlobalConfiguration.Configuration.Formatters.Clear();
            var formatter = new JsonMediaTypeFormatter();
            formatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            GlobalConfiguration.Configuration.Formatters.Add(formatter);
            GlobalConfiguration.Configuration.Formatters.Add(new PlainTextFormatter());
        }

        private static void InitializeRoutes()
        {
            DynamicApiRouteConfig.Register();
        }
    }
}
