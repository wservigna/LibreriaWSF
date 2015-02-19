﻿using System.Collections.Generic;

namespace WSF.Application.Navigation
{
    /// <summary>
    /// Represents an item in a <see cref="UserMenu"/>.
    /// </summary>
    public class UserMenuItem
    {
        /// <summary>
        /// Unique name of the menu item in the application. 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Icon of the menu item if exists.
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// Display name of the menu item.
        /// </summary>
        public string DisplayName { get; private set; }

        /// <summary>
        /// The URL to navigate when this menu item is selected.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Sub items of this menu item.
        /// </summary>
        public IList<UserMenuItem> Items { get; private set; }

        /// <summary>
        /// Creates a new <see cref="UserMenuItem"/> object.
        /// </summary>
        public UserMenuItem()
        {
            
        }

        /// <summary>
        /// Creates a new <see cref="UserMenuItem"/> object from given <see cref="MenuItemDefinition"/>.
        /// </summary>
        internal UserMenuItem(MenuItemDefinition menuItemDefinition)
        {
            Name = menuItemDefinition.Name;
            Icon = menuItemDefinition.Icon;
            DisplayName = menuItemDefinition.DisplayName.Localize();
            Url = menuItemDefinition.Url;
            Items = new List<UserMenuItem>();
        }
    }
}