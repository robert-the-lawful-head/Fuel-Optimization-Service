using System;

namespace FBOLinx.Core.BaseModels.Api
{
    public class BaseApiException : Exception, IApiException
    {
        /// <summary>
        /// Gets or sets the error code (HTTP status code)
        /// </summary>
        /// <value>The error code (HTTP status code).</value>
        public int ErrorCode { get; set; }

        /// <summary>
        /// Gets or sets the error content (body json object)
        /// </summary>
        /// <value>The error content (Http response body).</value>
        public Object ErrorContent { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiException"/> class.
        /// </summary>
        public BaseApiException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiException"/> class.
        /// </summary>
        /// <param name="errorCode">HTTP status code.</param>
        /// <param name="message">Error message.</param>
        public BaseApiException(int errorCode, string message) : base(message)
        {
            this.ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseApiException"/> class.
        /// </summary>
        /// <param name="errorCode">HTTP status code.</param>
        /// <param name="message">Error message.</param>
        /// <param name="errorContent">Error content.</param>
        public BaseApiException(int errorCode, string message, Object errorContent = null) : base(message)
        {
            this.ErrorCode = errorCode;
            this.ErrorContent = errorContent;
        }

    }
}
