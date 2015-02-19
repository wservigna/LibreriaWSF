using System;
using System.Collections.Generic;
using System.Linq;
using WSF.Reflection;

namespace WSF.Modules
{
    internal class DefaultModuleFinder : IModuleFinder
    {
        private readonly ITypeFinder _typeFinder;

        public DefaultModuleFinder(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
        }

        public ICollection<Type> FindAll()
        {
            return _typeFinder.Find(WSFModule.IsWSFModule).ToList();
        }
    }
}