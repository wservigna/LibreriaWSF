using WSF.Web.Configuration;

namespace WSF.Configuration.Startup
{
    /// <summary>
    /// Defines extension methods to <see cref="IModuleConfigurations"/> to allow to configure WSF Web module.
    /// </summary>
    public static class WSFWebConfigurationExtensions
    {
        /// <summary>
        /// Used to configure WSF Web module.
        /// </summary>
        public static IWSFWebModuleConfiguration WSFWeb(this IModuleConfigurations configurations)
        {
            return configurations.WSFConfiguration.GetOrCreate("Modules.WSF.Web", () => configurations.WSFConfiguration.IocManager.Resolve<IWSFWebModuleConfiguration>());
        }
    }
}