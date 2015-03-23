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
    
    /// Inherit from this class in global.asax instead of <see cref="HttpApplication"/> to be able to start WSF system.
    /// </summary>
    public abstract class WSFWebApplication : HttpApplication
    {
        /// <summary>
        /// Gets a reference to the <see cref="WSFBootstrapper"/> instance.
        /// </summary>
        private WSFBootstrapper WSFBootstrapper { get; set; }

        /// <summary>
        /// inicio de la appstartup. funciona en MVC4-MVC5
        /// </summary>
        protected virtual void Application_Start(object sender, EventArgs e)
        {
            
            IocManager.Instance.IocContainer.Register(Component.For<IAssemblyFinder>().ImplementedBy<WebAssemblyFinder>());

            
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
        /// Incio del request
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
        /// Estos metodos estan por implementar son base pero no tienen acciones
        /// </summary>
        protected virtual void Application_EndRequest(object sender, EventArgs e)
        {
        }

        protected virtual void Application_AuthenticateRequest(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Aqui se deberian poner los metodos para el manejo de errores
        /// </summary>

        protected virtual void Application_Error(object sender, EventArgs e)
        {

        }
    }
}
