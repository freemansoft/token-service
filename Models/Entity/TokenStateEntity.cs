using System;

namespace TokenService.Model.Entity
{
    /// <summary>
    /// Represents the current state of the token
    /// </summary>
    public class TokenStateEntity
    {
        /// <summary>
        /// Maximum number of times this token can be used
        /// </summary>
        public int maxUseCount = 1;
        /// <summary>
        /// The number of times this token has been used
        /// </summary>
        public int currentUseCount = 0;
        /// <summary>
        /// Length of time this token is valid.  Added to the initiation time
        /// </summary>
        public int expirationIntervalSec = 300;
        /// <summary>
        /// The time this token was created. Used to create expiration time
        /// </summary>
        public DateTime initiationTime = DateTime.Now;
        /// <summary>
        /// The expiration time for this token. Calculated using initiationTime + expirationIntervalSec
        /// </summary>
        public DateTime expirationTime = DateTime.MaxValue;
    }
}
