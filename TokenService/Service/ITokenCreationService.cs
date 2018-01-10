using TokenService.Model.Rest;

namespace TokenService.Service
{
    /// <summary>
    /// Token creation and managment service API
    /// </summary>
    public interface ITokenCreationService : ITokenService
    {
        /// <summary>
        /// Creates a token and returns it in the response. Throws an exception wrapping the response if there is an error.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        TokenCreateResponse CreateToken(TokenCreateRequest request);
    }
}
