using System.Linq;
using System.Threading.Tasks;
using WSF.Authorization.Roles;
using WSF.Dependency;
using WSF.MultiTenancy;
using Microsoft.AspNet.Identity;

namespace WSF.Authorization.Users
{
    /// <summary>
    /// Extends <see cref="UserManager{TRole,TKey}"/> of ASP.NET Identity Framework.
    /// </summary>
    public abstract class WSFUserManager<TTenant, TRole, TUser> : UserManager<TUser, long>, ITransientDependency
        where TTenant : WSFTenant<TTenant, TUser>
        where TRole : WSFRole<TTenant, TUser>
        where TUser : WSFUser<TTenant, TUser>
    {
        private readonly WSFRoleManager<TTenant, TRole, TUser> _roleManager;

        protected WSFUserManager(WSFUserStore<TTenant, TRole, TUser> userStore, WSFRoleManager<TTenant, TRole, TUser> roleManager)
            : base(userStore)
        {
            _roleManager = roleManager;
        }

        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public async Task<bool> IsGrantedAsync(long userId, string permissionName)
        {
            foreach (var role in await GetRolesAsync(userId))
            {
                if (await _roleManager.HasPermissionAsync(role, permissionName))
                {
                    return true;
                }
            }

            return false;
        }
    }
}