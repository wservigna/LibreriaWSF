namespace WSF.Configuration.Startup
{
    /// <summary>
    /// Used to provide a way to configure modules.
    /// Create entension methods to this class to be used over <see cref="IWSFStartupConfiguration.Modules"/> object.
    /// </summary>
    public interface IModuleConfigurations
    {
        /// <summary>
        /// Gets the WSF configuration object.
        /// </summary>
        IWSFStartupConfiguration WSFConfiguration { get; }
    }
}