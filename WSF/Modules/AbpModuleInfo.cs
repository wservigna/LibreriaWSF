using System;
using System.Collections.Generic;
using System.Reflection;

namespace WSF.Modules
{
    /// <summary>
    /// Used to store all needed information for a module.
    /// </summary>
    internal class WSFModuleInfo
    {
        /// <summary>
        /// The assembly which contains the module definition.
        /// </summary>
        public Assembly Assembly { get; private set; }

        /// <summary>
        /// Type of the module.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Instance of the module.
        /// </summary>
        public WSFModule Instance { get; private set; }

        /// <summary>
        /// All dependent modules of this module.
        /// </summary>
        public List<WSFModuleInfo> Dependencies { get; private set; }

        /// <summary>
        /// Creates a new WSFModuleInfo object.
        /// </summary>
        /// <param name="instance"></param>
        public WSFModuleInfo(WSFModule instance)
        {
            Dependencies = new List<WSFModuleInfo>();
            Type = instance.GetType();
            Instance = instance;
            Assembly = Type.Assembly;
        }

        public override string ToString()
        {
            return string.Format("{0}", Type.AssemblyQualifiedName);
        }
    }
}