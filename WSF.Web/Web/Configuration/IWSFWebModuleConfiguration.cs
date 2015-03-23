namespace WSF.Web.Configuration
{
    /// <summary>
    /// Used to configure WSF Web module.
    /// </summary>
    public interface IWSFWebModuleConfiguration
    {
        /// <summary>
        /// If this is set to true, all exception and details are sent directly to clients on an error.
        /// Default: false (WSF hides exception details from clients except special exceptions.)
        /// </summary>
        bool SendAllExceptionsToClients { get; set; }
    }
}