﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using WSF.Localization;

namespace WSF.Authorization
{
    /// <summary>
    /// Represents a permission.
    /// A permission is used to restrict functionalities of the application from unauthorized users.
    /// </summary>
    public sealed class Permission
    {
        /// <summary>
        /// Parent of this permission if one exists.
        /// If set, this permission can be granted only if parent is granted.
        /// </summary>
        public Permission Parent { get; private set; }

        /// <summary>
        /// Unique name of the permission.
        /// This is the key name to grant permissions.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Display name of the permission.
        /// This can be used to show permission to the user.
        /// </summary>
        public ILocalizableString DisplayName { get; private set; }

        /// <summary>
        /// A brief description for this permission.
        /// </summary>
        public ILocalizableString Description { get; private set; }

        /// <summary>
        /// Is this permission granted by default.
        /// Default value: false.
        /// </summary>
        public bool IsGrantedByDefault { get; private set; }

        /// <summary>
        /// List of child permissions. A child permission can be granted only if parent is granted.
        /// </summary>
        public IReadOnlyList<Permission> Children
        {
            get { return _children.ToImmutableList(); }
        }
        private readonly List<Permission> _children;

        /// <summary>
        /// Creates a new Permission.
        /// </summary>
        /// <param name="name">Unique name of the permission</param>
        /// <param name="displayName">Display name of the permission</param>
        /// <param name="isGrantedByDefault">Is this permission granted by default. Default value: false.</param>
        /// <param name="description">A brief description for this permission</param>
        public Permission(string name, ILocalizableString displayName, bool isGrantedByDefault = false, ILocalizableString description = null)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (displayName == null)
            {
                throw new ArgumentNullException("displayName");
            }

            Name = name;
            DisplayName = displayName;
            IsGrantedByDefault = isGrantedByDefault;
            Description = description;

            _children = new List<Permission>();
        }

        /// <summary>
        /// Adds a child permission.
        /// A child permission can be granted only if parent is granted.
        /// </summary>
        /// <returns>Returns newly created child permission</returns>
        public Permission CreateChildPermission(string name, ILocalizableString displayName, bool isGrantedByDefault = false, ILocalizableString description = null)
        {
            var permission = new Permission(name, displayName, isGrantedByDefault, description) { Parent = this };
            _children.Add(permission);
            return permission;
        }

        public override string ToString()
        {
            return string.Format("[Permission: {0}]", Name);
        }
    }
}
