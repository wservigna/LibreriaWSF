using WSF.Web.Configuration;

namespace WSF.Configuration.Startup
{
    
    public static class WSFWebConfigurationExtensions
    {
        /// <summary>
        /// Usador para configurar los modulos WSF Web.
        /// </summary>
        public static IWSFWebModuleConfiguration WSFWeb(this IModuleConfigurations configurations)
        {
            return configurations.WSFConfiguration.GetOrCreate("Modules.WSF.Web", () => configurations.WSFConfiguration.IocManager.Resolve<IWSFWebModuleConfiguration>());
        }
    }
}