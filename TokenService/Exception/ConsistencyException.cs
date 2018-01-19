using TokenService.Model.Dto;

namespace TokenService.Exception
{
    /// <summary>
    /// There is some error in the token configuration itself. expected values did not align
    /// </summary>
    public class ConsistencyException : TokenServiceException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceResponse"></param>
        public ConsistencyException(string message, TokenResponse serviceResponse) : base(message, serviceResponse)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="serviceResponse"></param>
        public ConsistencyException(string message, System.Exception innerException, TokenResponse serviceResponse) : base(message, innerException, serviceResponse)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        public ConsistencyException() : base()
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        public ConsistencyException(string message) : base(message)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ConsistencyException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

    }
}
