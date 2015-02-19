﻿using System.Web;
using WSF.Dependency;
using Castle.Core;

namespace WSF.Domain.Uow.Web
{
    /// <summary>
    /// Implements <see cref="ICurrentUnitOfWorkProvider"/> using <see cref="HttpContext.Current"/>.
    /// Fallbacks to <see cref="CallContextCurrentUnitOfWorkProvider"/> if <see cref="HttpContext.Current"/> is invalid.
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
                    return CallContextCurrentUnitOfWorkProvider.StaticUow; //TODO: Can inject it?
                }

                return HttpContext.Current.Items[ContextKey] as IUnitOfWork;
            }

            set
            {
                if (HttpContext.Current == null)
                {
                    CallContextCurrentUnitOfWorkProvider.StaticUow = value; //TODO: Can inject it?
                    return;
                }

                HttpContext.Current.Items[ContextKey] = value;
            }
        }
    }
}
