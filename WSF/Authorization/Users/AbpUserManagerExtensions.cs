using System;
using WSF.Authorization.Roles;
using WSF.MultiTenancy;
using WSF.Threading;

namespace WSF.Authorization.Users
{
    /// <summary>
    /// Extension methods for <see cref="WSFUserManager{TTenant,TRole,TUser}"/>.
    /// </summary>
    public static class WSFUserManagerExtensions
    {
        /// <summary>
        /// Check whether a user is granted for a permission.
        /// </summary>
        /// <param name="manager">User manager</param>
        /// <param name="userId">User id</param>
        /// <param name="permissionName">Permission name</param>
        public static bool IsGranted<TTenant, TRole, TUser>(WSFUserManager<TTenant, TRole, TUser> manager, long userId, string permissionName)
            where TTenant : WSFTenant<TTenant, TUser>
            where TRole : WSFRole<TTenant, TUser>
            where TUser : WSFUser<TTenant, TUser>
        {
            if (manager == null)
            {
                throw new ArgumentNullException("manager");
            }

            return AsyncHelper.RunSync(() => manager.IsGrantedAsync(userId, permissionName));
        }
    }
}