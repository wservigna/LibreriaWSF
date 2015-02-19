using System;
using System.Linq;
using WSF.Collections.Extensions;
using WSF.Dependency;
using WSF.Runtime.Validation;
using Castle.Core.Logging;

namespace WSF.Logging
{
    /// <summary>
    /// This class can be used to write logs from somewhere where it's a little hard to get a reference to the <see cref="ILogger"/>.
    /// Normally, get <see cref="ILogger"/> using property injection.
    /// </summary>
    public  class LogHelper
    {
        /// <summary>
        /// A reference to the logger.
        /// </summary>
        public static ILogger Logger { get; private set; }

        static LogHelper()
        {
            Logger = IocManager.Instance.IsRegistered(typeof (ILogger))
                ? IocManager.Instance.Resolve<ILogger>()
                : NullLogger.Instance;
        }

        public static void LogException(Exception ex)
        {
            Logger.Error(ex.ToString(), ex);
            LogValidationErrors(ex);
        }

        private static void LogValidationErrors(Exception exception)
        {
            if (exception is AggregateException && exception.InnerException != null)
            {
                var aggException = exception as AggregateException;
                if (aggException.InnerException is WSFValidationException)
                {
                    exception = aggException.InnerException;
                }
            }

            if (!(exception is WSFValidationException))
            {
                return;
            }

            var validationException = exception as WSFValidationException;
            if (validationException.ValidationErrors.IsNullOrEmpty())
            {
                return;
            }

            Logger.Warn("There are " + validationException.ValidationErrors.Count + " validation errors:");
            foreach (var validationResult in validationException.ValidationErrors)
            {
                var memberNames = "";
                if (validationResult.MemberNames != null && validationResult.MemberNames.Any())
                {
                    memberNames = " (" + string.Join(", ", validationResult.MemberNames) + ")";
                }

                Logger.Warn(validationResult.ErrorMessage + memberNames);
            }
        }
    }
}
