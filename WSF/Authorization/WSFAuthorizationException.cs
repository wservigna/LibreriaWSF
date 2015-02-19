using System;
using System.Runtime.Serialization;

namespace WSF.Authorization
{
    /// <summary>
    /// This exception is thrown on an unauthorized request.
    /// </summary>
    [Serializable]
    public class WSFAuthorizationException : WSFException
    {
        /// <summary>
        /// Creates a new <see cref="WSFAuthorizationException"/> object.
        /// </summary>
        public WSFAuthorizationException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="WSFAuthorizationException"/> object.
        /// </summary>
        public WSFAuthorizationException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WSFAuthorizationException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        public WSFAuthorizationException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WSFAuthorizationException"/> object.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Inner exception</param>
        public WSFAuthorizationException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}