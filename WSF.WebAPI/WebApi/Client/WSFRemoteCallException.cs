using System;
using System.Runtime.Serialization;
using WSF.Web.Models;

namespace WSF.WebApi.Client
{
    /// <summary>
    /// This exception is thrown when a remote method call made and remote application sent an error message.
    /// </summary>
    [Serializable]
    public class WSFRemoteCallException : WSFException
    {
        /// <summary>
        /// Remote error information.
        /// </summary>
        public ErrorInfo ErrorInfo { get; set; }

        /// <summary>
        /// Creates a new <see cref="WSFException"/> object.
        /// </summary>
        public WSFRemoteCallException()
        {

        }

        /// <summary>
        /// Creates a new <see cref="WSFException"/> object.
        /// </summary>
        public WSFRemoteCallException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {

        }

        /// <summary>
        /// Creates a new <see cref="WSFException"/> object.
        /// </summary>
        /// <param name="errorInfo">Exception message</param>
        public WSFRemoteCallException(ErrorInfo errorInfo)
            : base(errorInfo.Message)
        {
            ErrorInfo = errorInfo;
        }
    }
}
