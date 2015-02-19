namespace WSF.Configuration.Startup
{
    internal class ModuleConfigurations : IModuleConfigurations
    {
        public IWSFStartupConfiguration WSFConfiguration { get; private set; }

        public ModuleConfigurations(IWSFStartupConfiguration WSFConfiguration)
        {
            WSFConfiguration = WSFConfiguration;
        }
    }
}