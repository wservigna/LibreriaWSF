namespace WSF.Runtime.Session
{
    /// <summary>
    /// Implements null object pattern for <see cref="IWSFSession"/>.
    /// </summary>
    public class NullWSFSession : IWSFSession
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullWSFSession Instance { get { return SingletonInstance; } }
        private static readonly NullWSFSession SingletonInstance = new NullWSFSession();

        /// <inheritdoc/>
        public long? UserId { get { return null; } }

        /// <inheritdoc/>
        public int? TenantId { get { return null; } }

        private NullWSFSession()
        {

        }
    }
}