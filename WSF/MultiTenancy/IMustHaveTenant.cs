using System.ComponentModel.DataAnnotations.Schema;
using WSF.Authorization.Users;
using WSF.Domain.Entities;

namespace WSF.MultiTenancy
{
    /// <summary>
    /// Implement this interface for an entity which must have Tenant.
    /// </summary>
    public interface IMustHaveTenant<TTenant, TUser> : IMustHaveTenant, IFilterByTenant
        where TTenant : WSFTenant<TTenant, TUser>
        where TUser : WSFUser<TTenant, TUser>
    {
        /// <summary>
        /// Tenant.
        /// </summary>
        TTenant Tenant { get; set; }
    }
}
