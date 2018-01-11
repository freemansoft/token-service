using TokenService.Model.Rest;

namespace TokenService.Exception
{
    /// <summary>
    /// 
    /// </summary>
    public class TokenServiceException : System.Exception
    {
        /// <summary>
        /// 
        /// </summary>
        public TokenResponse ServiceResponse { get; private set; }

        /// <summary>
        /// These token service exceptions are expected to wrap a token response that will get returned to callers
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceResponse"></param>
        public TokenServiceException(string message, TokenResponse serviceResponse) : base(message)
        {
            this.ServiceResponse = serviceResponse;
        }

        /// <summary>
        /// These token service exceptions are expected to wrap a token response that will get returned to callers
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="serviceResponse"></param>
        public TokenServiceException(string message, System.Exception innerException, TokenResponse serviceResponse) : base(message, innerException)
        {
            this.ServiceResponse = serviceResponse;
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        public TokenServiceException() : base()
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        public TokenServiceException(string message) : base(message)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public TokenServiceException(string message, System.Exception innerException) : base(message, innerException)
        {
        }
    }
}
