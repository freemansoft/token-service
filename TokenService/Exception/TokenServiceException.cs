using TokenService.Model.Rest;

namespace TokenService
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
        public TokenServiceException(string message, TokenResponse serviceResponse): base(message)
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

    }
}
