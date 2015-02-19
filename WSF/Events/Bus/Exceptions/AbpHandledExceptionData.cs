using System;

namespace WSF.Events.Bus.Exceptions
{
    /// <summary>
    /// This type of events are used to notify for exceptions handled by WSF infrastructure.
    /// </summary>
    public class WSFHandledExceptionData : ExceptionData
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="exception">Exception object</param>
        public WSFHandledExceptionData(Exception exception)
            : base(exception)
        {

        }
    }
}