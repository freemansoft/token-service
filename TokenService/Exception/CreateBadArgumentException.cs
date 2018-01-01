using TokenService.Model.Rest;

namespace TokenService.Exception
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateBadArgumentException : TokenServiceException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceResponse"></param>
        public CreateBadArgumentException(string message, TokenResponse serviceResponse) : base(message, serviceResponse)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="serviceResponse"></param>
        public CreateBadArgumentException(string message, System.Exception innerException, TokenResponse serviceResponse) : base(message, innerException, serviceResponse)
        {
        }
    }
}
