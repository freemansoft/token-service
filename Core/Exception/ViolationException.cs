using TokenService.Model.Dto;

namespace TokenService.Core.Exception
{
    /// <summary>
    /// validation failed due to url, count, effectiveDate or expiration date
    /// </summary>
    public class ViolationException : TokenServiceException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceResponse"></param>
        public ViolationException(string message, TokenResponse serviceResponse) : base(message, serviceResponse)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="serviceResponse"></param>
        public ViolationException(string message, System.Exception innerException, TokenResponse serviceResponse) : base(message, innerException, serviceResponse)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        public ViolationException() : base()
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        public ViolationException(string message) : base(message)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ViolationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

    }
}
