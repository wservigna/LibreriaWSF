using System.Reflection;
using System.Web;
using WSF.Localization.Sources.Xml;
using WSF.Modules;
using WSF.Web.Configuration;

namespace WSF.Web
{
    /// <summary>
    /// This module is used to use WSF in ASP.NET web applications.
    /// </summary>
    public class WSFWebModule : WSFModule
    {
        /// <inheritdoc/>
        public override void PreInitialize()
        {
            if (HttpContext.Current != null)
            {
                XmlLocalizationSource.RootDirectoryOfApplication = HttpContext.Current.Server.MapPath("~");
            }

            IocManager.Register<IWSFWebModuleConfiguration, WSFWebModuleConfiguration>();
        }

        /// <inheritdoc/>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
            Configuration.Localization.Sources.Add(new XmlLocalizationSource("WSFWeb", "Localization\\WSFWeb"));
        }
    }
}
