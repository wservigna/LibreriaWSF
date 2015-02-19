using WSF.Authorization.Users;
using WSF.Domain.Entities;

namespace WSF.MultiTenancy
{
    /// <summary>
    /// Implement this interface for an entity which may have Tenant.
    /// </summary>
    public interface IMayHaveTenant<TTenant, TUser> : IMayHaveTenant
        where TTenant : WSFTenant<TTenant, TUser>
        where TUser : WSFUser<TTenant, TUser>
    {
        /// <summary>
        /// Tenant.
        /// </summary>
        TTenant Tenant { get; set; }
    }
}