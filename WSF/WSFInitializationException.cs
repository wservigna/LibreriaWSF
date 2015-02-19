using System;
using System.Runtime.Serialization;

namespace WSF
{
    /// <summary>
    /// This exception is thrown if a problem on WSF initialization progress.
    /// </summary>
    [Serializable]
    public class WSFInitializationException : WSFException
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public WSFInitializationException()
        {

        }

        /// <summary>
        /// Constructor for serializing.
        /// </summary>
        public WSFInitializationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        public WSFInitializationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public WSFInitializationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
