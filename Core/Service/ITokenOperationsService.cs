using TokenService.Model.Dto;

namespace TokenService.Core.Service
{
    /// <summary>
    /// Token creation and managment service API
    /// </summary>
    public interface ITokenOperationsService : ITokenService
    {
        /// <summary>
        /// Creates a token and returns it in the response. Throws an exception wrapping the response if there is an error.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TokenCreateResponse CreateToken(TokenCreateRequest request);

        /// <summary>
        /// Validates the passed in token should be honored
        /// Returns the response.  Throws an exception containing a response if it fails
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TokenValidateResponse ValidateToken(TokenValidateRequest request);

    }

}
