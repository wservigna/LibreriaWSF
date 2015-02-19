using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using WSF.Authorization.Users;
using WSF.Configuration;
using WSF.Domain.Entities.Auditing;

namespace WSF.MultiTenancy
{
    /// <summary>
    /// Represents a Tenant of the application.
    /// </summary>
    [Table("WSFTenants")]
    public class WSFTenant<TTenant, TUser> : AuditedEntity<int, TUser>
        where TUser : WSFUser<TTenant,TUser>
        where TTenant : WSFTenant<TTenant, TUser>
    {
        /// <summary>
        /// Tenancy name. This property is the UNIQUE name of this Tenant.
        /// It can be used as subdomain name in a web application.
        /// </summary>
        public virtual string TenancyName { get; set; }

        /// <summary>
        /// Display name of the Tenant.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Defined settings for this tenant.
        /// </summary>
        [ForeignKey("TenantId")]
        public virtual ICollection<Setting> Settings { get; set; }

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        public WSFTenant()
        {
            
        }

        /// <summary>
        /// Creates a new tenant.
        /// </summary>
        /// <param name="tenancyName">UNIQUE name of this Tenant</param>
        /// <param name="name">Display name of the Tenant</param>
        public WSFTenant(string tenancyName, string name)
        {
            TenancyName = tenancyName;
            Name = name;
        }
    }
}
