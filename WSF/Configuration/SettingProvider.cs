﻿using System.Collections.Generic;
using WSF.Dependency;

namespace WSF.Configuration
{
    /// <summary>
    /// Implement this interface to define settings for a module/application.
    /// </summary>
    public abstract class SettingProvider : ISingletonDependency
    {
        /// <summary>
        /// Gets all setting definitions provided by this provider.
        /// </summary>
        /// <returns>List of settings</returns>
        public virtual IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new SettingDefinition[0];
        }
    }
}