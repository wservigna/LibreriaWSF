using WSF.Modules;
using System.Reflection;
using WSF.Reflection;
using Castle.Core.Logging;


namespace WSF.AutoMapper
{
    public class WSFAutoMapperModule : WSFModule
    {
        public ILogger Logger { get; set; }

        private readonly ITypeFinder _typeFinder;

        private static bool _createdMappingsBefore;

        public WSFAutoMapperModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
            Logger = NullLogger.Instance;
        }

        public override void PreInitialize()
        {
            CreateMappings();
        }

        private void CreateMappings()
        {
            //We should prevent duplicate mapping in an application, since AutoMapper is static.
            if (_createdMappingsBefore)
            {
                return;
            }

            _createdMappingsBefore = true;

            var types = _typeFinder.Find(type =>
                type.IsDefined(typeof(AutoMapAttribute)) ||
                type.IsDefined(typeof(AutoMapFromAttribute)) ||
                type.IsDefined(typeof(AutoMapToAttribute))
                );

            Logger.DebugFormat("Found {0} classes defines auto mapping attributes", types.Length);
            foreach (var type in types)
            {
                Logger.Debug(type.FullName);
                AutoMapperHelper.CreateMap(type);
            }
        }
    }
}
