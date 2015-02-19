using System;
using System.Runtime.Serialization;

namespace WSF
{
    /// <summary>
    /// Base exception type for those are thrown by WSF system for WSF specific exceptions.
    /// </summary>
    [Serializable]
    public class WSFException : Exception
    {
        /// <summary>
        /// Creates a new <see cref="WSFException"/> object.
        /// </summary>
        public WSFException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="WSFException"/> object.
        /// </summary>
        public WSFException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WSFException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public WSFException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WSFException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public WSFException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
