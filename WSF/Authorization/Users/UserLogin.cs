using System.ComponentModel.DataAnnotations.Schema;
using WSF.Domain.Entities;

namespace WSF.Authorization.Users
{
    /// <summary>
    /// Used to store a User Login for external Login services.
    /// </summary>
    [Table("WSFUserLogins")]
    public class UserLogin : Entity<long>
    {
        /// <summary>
        /// Id of the User.
        /// </summary>
        public virtual long UserId { get; set; }

        /// <summary>
        /// Login Provider.
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Key in the <see cref="LoginProvider"/>.
        /// </summary>
        public virtual string ProviderKey { get; set; }
    }
}
