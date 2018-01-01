using TokenService.Model.Rest;

namespace TokenService
{
    /// <summary>
    /// 
    /// </summary>
    public class ValidateFailedException : TokenServiceException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceResponse"></param>
        public ValidateFailedException(string message, TokenResponse serviceResponse) : base(message, serviceResponse)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="serviceResponse"></param>
        public ValidateFailedException(string message, System.Exception innerException, TokenResponse serviceResponse) : base(message, innerException, serviceResponse)
        {
        }
    }
}
