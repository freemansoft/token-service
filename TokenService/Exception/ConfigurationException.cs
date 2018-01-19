using TokenService.Model.Dto;

namespace TokenService.Exception
{
    /// <summary>
    /// 
    /// </summary>
    public class ConfigurationException : TokenServiceException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="serviceResponse"></param>
        public ConfigurationException(string message, TokenResponse serviceResponse) : base(message, serviceResponse)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="serviceResponse"></param>
        public ConfigurationException(string message, System.Exception innerException, TokenResponse serviceResponse) : base(message, innerException, serviceResponse)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        public ConfigurationException() : base()
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        public ConfigurationException(string message) : base(message)
        {
        }

        /// <summary>
        /// override standard Excepton constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ConfigurationException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

    }
}
