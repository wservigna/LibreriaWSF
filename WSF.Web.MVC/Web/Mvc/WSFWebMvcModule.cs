using System.Reflection;
using System.Web.Mvc;
using WSF.Modules;
using WSF.Web.Mvc.Controllers;

namespace WSF.Web.Mvc
{
    /// <summary>
    /// This module is used to build ASP.NET MVC web sites using WSF.
    /// </summary>
    [DependsOn(typeof(WSFWebModule))]
    public class WSFWebMvcModule : WSFModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new ControllerConventionalRegistrar());
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            ControllerBuilder.Current.SetControllerFactory(new WindsorControllerFactory(IocManager.IocContainer.Kernel));
            GlobalFilters.Filters.Add(new WSFHandleErrorAttribute());
        }
    }
}
