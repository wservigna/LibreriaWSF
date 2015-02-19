using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using WSF.Collections.Extensions;
using WSF.Configuration.Startup;
using WSF.Dependency;
using WSF.Localization;
using WSF.Runtime.Session;
using Castle.Core.Logging;

namespace WSF.Authorization
{
    /// <summary>
    /// Permission manager.
    /// </summary>
    internal class PermissionManager : IPermissionManager, ISingletonDependency, IPermissionDefinitionContext
    {
        public ILogger Logger { get; set; }

        public IWSFSession WSFSession { get; set; }

        private readonly IIocManager _iocManager;
        private readonly IAuthorizationConfiguration _authorizationConfiguration;

        private readonly PermissionDictionary _permissions;
        
        /// <summary>
        /// Constructor.
        /// </summary>
        public PermissionManager(IIocManager iocManager, IAuthorizationConfiguration authorizationConfiguration)
        {
            Logger = NullLogger.Instance;
            WSFSession = NullWSFSession.Instance;

            _iocManager = iocManager;
            _authorizationConfiguration = authorizationConfiguration;

            _permissions = new PermissionDictionary();
        }

        public void Initialize()
        {
            foreach (var providerType in _authorizationConfiguration.Providers)
            {
                CreatePermissionProvider(providerType).SetPermissions(this);
            }

            _permissions.AddAllPermissions();
        }

        public Permission CreatePermission(string name, ILocalizableString displayName, bool isGrantedByDefault = false, ILocalizableString description = null)
        {
            if (_permissions.ContainsKey(name))
            {
                throw new WSFException("There is already a permission with name: " + name);
            }

            var permission = new Permission(name, displayName, isGrantedByDefault, description);
            _permissions[permission.Name] = permission;
            return permission;
        }

        public Permission GetPermission(string name)
        {
            var permission = _permissions.GetOrDefault(name);
            if (permission == null)
            {
                throw new WSFException("There is no permission with name: " + name);
            }

            return permission;
        }

        public Permission GetPermissionOrNull(string name)
        {
            return _permissions.GetOrDefault(name);
        }

        public IReadOnlyList<Permission> GetAllPermissions()
        {
            return _permissions.Values.ToImmutableList();
        }

        private AuthorizationProvider CreatePermissionProvider(Type providerType)
        {
            if (!_iocManager.IsRegistered(providerType))
            {
                _iocManager.Register(providerType);
            }

            return (AuthorizationProvider) _iocManager.Resolve(providerType);
        }
    }
}