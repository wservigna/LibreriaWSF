using System.Data.Entity.Infrastructure.Interception;
using System.Reflection;
using WSF.Collections.Extensions;
using WSF.EntityFramework.Dependency;
using WSF.EntityFramework.Repositories;
using WSF.EntityFramework.SoftDeleting;
using WSF.EntityFramework.Uow;
using WSF.Modules;
using WSF.Reflection;
using Castle.Core.Logging;
using Castle.MicroKernel.Registration;

namespace WSF.EntityFramework
{
    /// <summary>
    /// This module is used to implement "Data Access Layer" in EntityFramework.
    /// </summary>
    public class WSFEntityFrameworkModule : WSFModule
    {
        public ILogger Logger { get; set; }

        private readonly ITypeFinder _typeFinder;

        public WSFEntityFrameworkModule(ITypeFinder typeFinder)
        {
            _typeFinder = typeFinder;
            Logger = NullLogger.Instance;
        }

        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new EntityFrameworkConventionalRegisterer());
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            IocManager.IocContainer.Register(
                Component.For(typeof (IDbContextProvider<>))
                    .ImplementedBy(typeof (UnitOfWorkDbContextProvider<>))
                    .LifestyleTransient()
                );
            
            RegisterGenericRepositories();

            DbInterception.Add(new SoftDeleteInterceptor());
        }

        private void RegisterGenericRepositories()
        {
            var dbContextTypes =
                _typeFinder.Find(type =>
                    type.IsPublic &&
                    !type.IsAbstract &&
                    type.IsClass &&
                    typeof(WSFDbContext).IsAssignableFrom(type)
                    );

            if (dbContextTypes.IsNullOrEmpty())
            {
                Logger.Warn("No class found derived from WSFDbContext.");
                return;
            }

            foreach (var dbContextType in dbContextTypes)
            {
                EntityFrameworkGenericRepositoryRegistrar.RegisterForDbContext(dbContextType, IocManager);
            }
        }
    }
}
