using System.Threading.Tasks;
using WSF.Authorization.Roles;
using WSF.Authorization.Users;
using WSF.Dependency;
using WSF.MultiTenancy;
using WSF.Runtime.Session;
using Castle.Core.Logging;

namespace WSF.Authorization
{
    /// <summary>
    /// Application should inherit this class to implement <see cref="IPermissionChecker"/>.
    /// </summary>
    /// <typeparam name="TTenant"></typeparam>
    /// <typeparam name="TRole"></typeparam>
    /// <typeparam name="TUser"></typeparam>
    public abstract class PermissionChecker<TTenant, TRole, TUser> : IPermissionChecker, ITransientDependency
        where TRole : WSFRole<TTenant, TUser> 
        where TUser : WSFUser<TTenant, TUser> 
        where TTenant : WSFTenant<TTenant, TUser>
    {
        private readonly WSFUserManager<TTenant,TRole, TUser> _userManager;

        public ILogger Logger { get; set; }

        public IWSFSession WSFSession { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        protected PermissionChecker(WSFUserManager<TTenant, TRole, TUser> userManager)
        {
            _userManager = userManager;
    
            Logger = NullLogger.Instance;
            WSFSession = NullWSFSession.Instance;
        }

        public async Task<bool> IsGrantedAsync(string permissionName)
        {
            return WSFSession.UserId.HasValue && await _userManager.IsGrantedAsync(WSFSession.UserId.Value, permissionName);
        }

        public async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            return await _userManager.IsGrantedAsync(userId,permissionName);
        }
    }
}
