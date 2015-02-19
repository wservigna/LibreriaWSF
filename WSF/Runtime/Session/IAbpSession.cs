namespace WSF.Runtime.Session
{
    /// <summary>
    /// Defines some session information that can be useful for applications.
    /// </summary>
    public interface IWSFSession
    {
        /// <summary>
        /// Gets current UserId of null.
        /// </summary>
        long? UserId { get; }

        /// <summary>
        /// Gets current TenantId or null.
        /// </summary>
        int? TenantId { get; }
    }
}
