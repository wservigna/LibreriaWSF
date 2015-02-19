using System;
using System.Collections.Generic;

namespace WSF.Modules
{
    /// <summary>
    /// This interface is responsible to find all modules (derived from <see cref="WSFModule"/>)
    /// in the application.
    /// </summary>
    public interface IModuleFinder
    {
        /// <summary>
        /// Finds all modules.
        /// </summary>
        /// <returns>List of modules</returns>
        ICollection<Type> FindAll();
    }
}