﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace WSF.Configuration
{
    /// <summary>
    /// This is the main interface that must be implemented to be able to load/change values of settings.
    /// </summary>
    public interface ISettingManager
    {
        /// <summary>
        /// Gets current value of a setting.
        /// It gets the setting value, overwritten by application and the current user if exists.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Current value of the setting</returns>
        Task<string> GetSettingValueAsync(string name);

        /// <summary>
        /// Gets value of a setting.
        /// </summary>
        /// <typeparam name="T">Type of the setting to get</typeparam>
        /// <param name="name">Unique name of the setting</param>
        /// <returns>Value of the setting</returns>
        Task<T> GetSettingValueAsync<T>(string name);

        /// <summary>
        /// Gets current values of all settings.
        /// It gets all setting values, overwritten by application and the current user if exists.
        /// </summary>
        /// <returns>List of setting values</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesAsync();

        /// <summary>
        /// Gets a list of all setting values specified for the application.
        /// It returns only settings those are explicitly set for the application.
        /// If a setting's default value is used, it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValuesAsync"/> method.
        /// </summary>
        /// <returns>List of setting values</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForApplicationAsync();

        /// <summary>
        /// Gets a list of all setting values specified for a tenant.
        /// It returns only settings those are explicitly set for the tenant.
        /// If a setting's default value is used, it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValuesAsync"/> method.
        /// </summary>
        /// <param name="tenantId">Tenant to get settings</param>
        /// <returns>List of setting values</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForTenantAsync(int tenantId);

        /// <summary>
        /// Gets a list of all setting values specified for a user.
        /// It returns only settings those are explicitly set for the user.
        /// If a setting's value is not set for the user (for example if user uses the default value), it's not included the result list.
        /// If you want to get current values of all settings, use <see cref="GetAllSettingValuesAsync"/> method.
        /// </summary>
        /// <param name="userId">User to get settings</param>
        /// <returns>All settings of the user</returns>
        Task<IReadOnlyList<ISettingValue>> GetAllSettingValuesForUserAsync(long userId);

        /// <summary>
        /// Changes setting for the application level.
        /// </summary>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        Task ChangeSettingForApplicationAsync(string name, string value);

        /// <summary>
        /// Changes setting for a Tenant.
        /// </summary>
        /// <param name="tenantId">TenantId</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        Task ChangeSettingForTenantAsync(int tenantId, string name, string value);

        /// <summary>
        /// Changes setting for a user.
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="name">Unique name of the setting</param>
        /// <param name="value">Value of the setting</param>
        Task ChangeSettingForUserAsync(long userId, string name, string value);
    }
}
