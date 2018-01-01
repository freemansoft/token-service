using TokenService.Model.Rest;

namespace TokenService.Service
{
    /// <summary>
    /// Token validation service API
    /// </summary>
    public interface ITokenValidationService
    {
        /// <summary>
        /// Validates the passed in token should be honored
        /// Returns the response.  Throws an exception containing a response if it fails
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TokenValidateResponse ValidateToken(TokenValidateRequest request);
    }
}
