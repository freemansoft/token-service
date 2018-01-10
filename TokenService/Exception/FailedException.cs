using TokenService.Model.Rest;

namespace TokenService
{
    /// <summary>
    /// 
    /// </summary>
    public class FailedException : TokenServiceException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceResponse"></param>
        public FailedException(string message, TokenResponse serviceResponse) : base(message, serviceResponse)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="serviceResponse"></param>
        public FailedException(string message, System.Exception innerException, TokenResponse serviceResponse) : base(message, innerException, serviceResponse)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        public FailedException() : base()
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        public FailedException(string message) : base(message)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public FailedException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

    }
}
