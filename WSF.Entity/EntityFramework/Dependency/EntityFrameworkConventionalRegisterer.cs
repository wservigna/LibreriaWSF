using System.Configuration;
using WSF.Configuration.Startup;
using WSF.Dependency;
using Castle.MicroKernel.Registration;

namespace WSF.EntityFramework.Dependency
{
    /// <summary>
    /// Registers classes derived from WSFDbContext with configurations.
    /// </summary>
    public class EntityFrameworkConventionalRegisterer : IConventionalDependencyRegistrar
    {
        public void RegisterAssembly(IConventionalRegistrationContext context)
        {
            context.IocManager.IocContainer.Register(
                Classes.FromAssembly(context.Assembly)
                    .IncludeNonPublicTypes()
                    .BasedOn<WSFDbContext>()
                    .WithServiceSelf()
                    .LifestyleTransient()
                    .Configure(c => c.DynamicParameters(
                        (kernel, dynamicParams) =>
                        {
                            var connectionString = GetNameOrConnectionStringOrNull(context.IocManager);
                            if (!string.IsNullOrWhiteSpace(connectionString))
                            {
                                dynamicParams["nameOrConnectionString"] = connectionString;
                            }
                        })));
        }

        private static string GetNameOrConnectionStringOrNull(IIocResolver iocResolver)
        {
            if (iocResolver.IsRegistered<IWSFStartupConfiguration>())
            {
                var defaultConnectionString = iocResolver.Resolve<IWSFStartupConfiguration>().DefaultNameOrConnectionString;
                if (!string.IsNullOrWhiteSpace(defaultConnectionString))
                {
                    return defaultConnectionString;
                }
            }

            if (ConfigurationManager.ConnectionStrings.Count == 1)
            {
                return ConfigurationManager.ConnectionStrings[0].Name;
            }

            if (ConfigurationManager.ConnectionStrings["Default"] != null)
            {
                return "Default";
            }

            return null;
        }
    }
}