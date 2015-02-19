using WSF.Authorization;
using WSF.Authorization.Interceptors;
using WSF.Dependency;
using WSF.Runtime.Validation.Interception;
using Castle.Core;

namespace WSF.Application.Services.Interceptors
{
    /// <summary>
    /// This class is used to register interceptors on the Application Layer.
    /// </summary>
    public static class ApplicationServiceInterceptorRegistrar
    {
        public static void Initialize(IIocManager iocManager)
        {
            iocManager.IocContainer.Kernel.ComponentRegistered += Kernel_ComponentRegistered;            
        }

        private static void Kernel_ComponentRegistered(string key, Castle.MicroKernel.IHandler handler)
        {
            if (typeof(IApplicationService).IsAssignableFrom(handler.ComponentModel.Implementation))
            {
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(ValidationInterceptor)));
                handler.ComponentModel.Interceptors.Add(new InterceptorReference(typeof(AuthorizationInterceptor))); 
            }
        }
    }
}