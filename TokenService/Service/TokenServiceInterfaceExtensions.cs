using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TokenService.Exception;
using TokenService.Model.Rest;

namespace TokenService.Service
{
    /// <summary>
    /// extension methods for ITokenService implementers
    /// </summary>
    public static class TokenServiceInterfaceExtensions
    {
        /// <summary>
        /// throws BadArgumentException if there is a problem with the token.
        /// Adds a TokenResponseMessage for each validation error
        /// </summary>
        /// <param name="myTokenService"></param>
        /// <param name="validationResults"></param>
        public static void RaiseValidationErrors(this ITokenService myTokenService, IEnumerable<ValidationResult> validationResults)
        {
            List<TokenResponseMessage> messages = new List<TokenResponseMessage>();
            foreach (ValidationResult oneResult in validationResults)
            {
                TokenResponseMessage message = new TokenResponseMessage(null, oneResult.ErrorMessage);
                messages.Add(message);
            }
            if (messages.Count > 0)
            {
                throw new BadArgumentException("Failed Object Validation", new TokenResponse(messages));
            }
        }
    }
}
