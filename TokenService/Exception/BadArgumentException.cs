using TokenService.Model.Rest;

namespace TokenService.Exception
{
    /// <summary>
    /// 
    /// </summary>
    public class BadArgumentException : TokenServiceException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceResponse"></param>
        public BadArgumentException(string message, TokenResponse serviceResponse) : base(message, serviceResponse)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="serviceResponse"></param>
        public BadArgumentException(string message, System.Exception innerException, TokenResponse serviceResponse) : base(message, innerException, serviceResponse)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        public BadArgumentException() : base()
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        public BadArgumentException(string message) : base(message)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public BadArgumentException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

    }
}
