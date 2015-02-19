using System;
using WSF.Configuration.Startup;
using WSF.Dependency;
using WSF.Dependency.Installers;
using WSF.Modules;

namespace WSF
{
    /// <summary>
    /// This is the main class that is responsible to start entire WSF system.
    /// Prepares dependency injection and registers core components needed for startup.
    /// It must be instantiated and initialized (see <see cref="Initialize"/>) first in an application.
    /// </summary>
    public class WSFBootstrapper : IDisposable
    {
        /// <summary>
        /// Gets IIocManager object used by this class.
        /// </summary>
        public IIocManager IocManager { get; private set; }

        /// <summary>
        /// Is this object disposed before?
        /// </summary>
        protected bool IsDisposed;

        private IWSFModuleManager _moduleManager;

        /// <summary>
        /// Creates a new <see cref="WSFBootstrapper"/> instance.
        /// </summary>
        public WSFBootstrapper()
            : this(Dependency.IocManager.Instance)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WSFBootstrapper"/> instance.
        /// </summary>
        /// <param name="iocManager">IIocManager that is used to bootstrap the WSF system</param>
        public WSFBootstrapper(IIocManager iocManager)
        {
            IocManager = iocManager;
        }

        /// <summary>
        /// Initializes the WSF system.
        /// </summary>
        public virtual void Initialize()
        {
            IocManager.IocContainer.Install(new WSFCoreInstaller());

            IocManager.Resolve<WSFStartupConfiguration>().Initialize();

            _moduleManager = IocManager.Resolve<IWSFModuleManager>();
            _moduleManager.InitializeModules();
        }

        /// <summary>
        /// Disposes the WSF system.
        /// </summary>
        public virtual void Dispose()
        {
            if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            if (_moduleManager != null)
            {
                _moduleManager.ShutdownModules();
            }
        }
    }
}
