using System;
using System.Globalization;
using System.Threading;
using System.Web;
using WSF.Dependency;
using WSF.Domain.Uow;
using WSF.Domain.Uow.Web;
using WSF.Localization;
using WSF.Reflection;
using Castle.MicroKernel.Registration;

namespace WSF.Web
{
    /// <summary>
    /// This class is used to simplify starting of WSF system using <see cref="WSFBootstrapper"/> class..
    /// Inherit from this class in global.asax instead of <see cref="HttpApplication"/> to be able to start WSF system.
    /// </summary>
    public abstract class WSFWebApplication : HttpApplication
    {
        /// <summary>
        /// Gets a reference to the <see cref="WSFBootstrapper"/> instance.
        /// </summary>
        private WSFBootstrapper WSFBootstrapper { get; set; }

        /// <summary>
        /// This method is called by ASP.NET system on web application's startup.
        /// </summary>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            //TODO: Use WSFBootstrapper.IocManager
            IocManager.Instance.IocContainer.Register(Component.For<IAssemblyFinder>().ImplementedBy<WebAssemblyFinder>());

            //TODO: Use WSFBootstrapper.IocManager
            //TODO: Move to WSFWebModule?
            IocManager.Instance.IocContainer.Register(Component.For<ICurrentUnitOfWorkProvider>().ImplementedBy<HttpContextCurrentUnitOfWorkProvider>());

            WSFBootstrapper = new WSFBootstrapper();
            WSFBootstrapper.Initialize();
        }

        protected virtual void Application_End(object sender, EventArgs e)
        {
            WSFBootstrapper.Dispose();
        }

        protected virtual void Session_Start(object sender, EventArgs e)
        {
        }

        protected virtual void Session_End(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// This method is called by ASP.NET system when a request starts.
        /// </summary>
        protected virtual void Application_BeginRequest(object sender, EventArgs e)
        {
            var langCookie = Request.Cookies["WSF.Localization.CultureName"];
            if (langCookie != null && GlobalizationHelper.IsValidCultureCode(langCookie.Value))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(langCookie.Value);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCookie.Value);
            }
        }

        /// <summary>
        /// This method is called by ASP.NET system when a request ends.
        /// </summary>
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {
        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        protected virtual void Application_Error(object sender, EventArgs e)
        {

        }
    }
}
