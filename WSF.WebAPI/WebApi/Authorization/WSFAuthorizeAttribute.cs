using System.Web.Http;
using System.Web.Http.Controllers;
using WSF.Authorization;
using WSF.Dependency;
using WSF.Logging;

namespace WSF.WebApi.Authorization
{
    /// <summary>
    /// This attribute is used on a method of an <see cref="ApiController"/>
    /// to make that method usable only by authorized users.
    /// </summary>
    public class WSFAuthorizeAttribute : AuthorizeAttribute, IWSFAuthorizeAttribute
    {
        /// <inheritdoc/>
        public string[] Permissions { get; set; }

        /// <inheritdoc/>
        public bool RequireAllPermissions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="WSFAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize</param>
        public WSFAuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }

        /// <inheritdoc/>
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            if (!base.IsAuthorized(actionContext))
            {
                return false;
            }

            try
            {
                //TODO: Use Async..?
                using (var authorizationAttributeHelper = IocManager.Instance.ResolveAsDisposable<IAuthorizeAttributeHelper>())
                {
                    authorizationAttributeHelper.Object.Authorize(this);
                }

                return true;
            }
            catch (WSFAuthorizationException ex)
            {
                LogHelper.Logger.Warn(ex.ToString(), ex);
                return false;
            }
        }
    }
}
