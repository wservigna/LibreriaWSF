using System.Web;
using WSF.Dependency;
using Castle.Core;

namespace WSF.Domain.Uow.Web
{
    /// <summary>
    /// Implementa <see cref="ICurrentUnitOfWorkProvider"/> usando <see cref="HttpContext.Current"/>.
    /// Fallbacks hacia <see cref="CallContextCurrentUnitOfWorkProvider"/> si <see cref="HttpContext.Current"/> is invalido
    /// </summary>
    public class HttpContextCurrentUnitOfWorkProvider : ICurrentUnitOfWorkProvider, ISingletonDependency
    {
        private const string ContextKey = "WSF.UnitOfWork.Current";

        [DoNotWire]
        public IUnitOfWork Current
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return CallContextCurrentUnitOfWorkProvider.StaticUow; 
                }

                return HttpContext.Current.Items[ContextKey] as IUnitOfWork;
            }

            set
            {
                if (HttpContext.Current == null)
                {
                    CallContextCurrentUnitOfWorkProvider.StaticUow = value; 
                    return;
                }

                HttpContext.Current.Items[ContextKey] = value;
            }
        }
    }
}
