using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WSF.Collections.Extensions;
using WSF.Dependency;
using WSF.Runtime.Session;
using WSF.Threading;

namespace WSF.Authorization
{
    internal class AuthorizeAttributeHelper : IAuthorizeAttributeHelper, ITransientDependency
    {
        public IWSFSession WSFSession { get; set; }

        public IPermissionChecker PermissionChecker { get; set; }

        public AuthorizeAttributeHelper()
        {
            WSFSession = NullWSFSession.Instance;
            PermissionChecker = NullPermissionChecker.Instance;
        }

        public async Task AuthorizeAsync(IEnumerable<IWSFAuthorizeAttribute> authorizeAttributes)
        {
            if (!WSFSession.UserId.HasValue)
            {
                throw new WSFAuthorizationException("No user logged in!");
            }

            foreach (var authorizeAttribute in authorizeAttributes)
            {
                if (authorizeAttribute.Permissions.IsNullOrEmpty())
                {
                    continue;
                }

                if (authorizeAttribute.RequireAllPermissions)
                {
                    foreach (var permissionName in authorizeAttribute.Permissions)
                    {
                        if (!await PermissionChecker.IsGrantedAsync(permissionName))
                        {
                            throw new WSFAuthorizationException(
                                "Required permissions are not granted. All of these permissions must be granted: " +
                                String.Join(", ", authorizeAttribute.Permissions)
                                );
                        }
                    }
                }
                else
                {
                    foreach (var permissionName in authorizeAttribute.Permissions)
                    {
                        if (await PermissionChecker.IsGrantedAsync(permissionName))
                        {
                            return; //Authorized
                        }
                    }

                    //Not authorized!
                    throw new WSFAuthorizationException(
                        "Required permissions are not granted. At least one of these permissions must be granted: " +
                        String.Join(", ", authorizeAttribute.Permissions)
                        );
                }
            }
        }

        public async Task AuthorizeAsync(IWSFAuthorizeAttribute authorizeAttribute)
        {
            await AuthorizeAsync(new[] { authorizeAttribute });
        }

        public void Authorize(IEnumerable<IWSFAuthorizeAttribute> authorizeAttributes)
        {
            AsyncHelper.RunSync(() => AuthorizeAsync(authorizeAttributes));
        }

        public void Authorize(IWSFAuthorizeAttribute authorizeAttribute)
        {
            Authorize(new[] { authorizeAttribute });
        }
    }
}