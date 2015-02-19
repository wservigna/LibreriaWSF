using System.Reflection;
using WSF.Application.Navigation;
using WSF.Application.Services.Interceptors;
using WSF.Authorization;
using WSF.Configuration;
using WSF.Configuration.Startup;
using WSF.Dependency;
using WSF.Domain.Uow;
using WSF.Events.Bus;
using WSF.Modules;

namespace WSF
{
    public sealed class WSFKernelModule : WSFModule
    {
        public override void PreInitialize()
        {
            IocManager.AddConventionalRegistrar(new BasicConventionalRegistrar());
            UnitOfWorkRegistrar.Initialize(IocManager);
            ApplicationServiceInterceptorRegistrar.Initialize(IocManager);
        }

        public override void Initialize()
        {
            base.Initialize();

            IocManager.IocContainer.Install(new EventBusInstaller(IocManager));

            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly(),
                new ConventionalRegistrationConfig
                {
                    InstallInstallers = false
                });
        }

        public override void PostInitialize()
        {
            RegisterMissingComponents();

            IocManager.Resolve<NavigationManager>().Initialize();
            IocManager.Resolve<PermissionManager>().Initialize();
            IocManager.Resolve<SettingDefinitionManager>().Initialize();
        }

        private void RegisterMissingComponents()
        {
            if (!IocManager.IsRegistered<IUnitOfWork>())
            {
                IocManager.Register<IUnitOfWork, NullUnitOfWork>(DependencyLifeStyle.Transient);
            }
        }
    }
}