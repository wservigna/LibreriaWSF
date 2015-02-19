﻿using System;
using WSF.Application.Services;

namespace WSF.Authorization
{
    /// <summary>
    /// This attribute is used on a method of an Application Service (A class that implements <see cref="IApplicationService"/>)
    /// to make that method usable only by authorized users.
    /// </summary>
    public class WSFAuthorizeAttribute : Attribute, IWSFAuthorizeAttribute
    {
        /// <summary>
        /// A list of permissions to authorize.
        /// </summary>
        public string[] Permissions { get; private set; }

        /// <summary>
        /// If this property is set to true, all of the <see cref="Permissions"/> must be granted.
        /// If it's false, at least one of the <see cref="Permissions"/> must be granted.
        /// Default: false.
        /// </summary>
        public bool RequireAllPermissions { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="WSFAuthorizeAttribute"/> class.
        /// </summary>
        /// <param name="permissions">A list of permissions to authorize</param>
        public WSFAuthorizeAttribute(params string[] permissions)
        {
            Permissions = permissions;
        }
    }
}
