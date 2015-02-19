﻿using System.Threading.Tasks;

namespace WSF.Web.Authorization
{
    /// <summary>
    /// This class is used to build and cache authorization script.
    /// </summary>
    public interface IAuthorizationScriptManager
    {
        /// <summary>
        /// Gets Javascript that contains all authorization information.
        /// </summary>
        Task<string> GetScriptAsync();
    }
}
