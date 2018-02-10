namespace TokenService.Core.Service
{
    /// <summary>
    /// Cryptography and security settings from the appsettings file chain.
    /// Initially created for managing JWT secret.
    /// </summary>
    public class CryptographySettings
    {
        /// <summary>
        /// The 128 char minimum lenght cryptograph key
        /// </summary>
        public string JwtSecret { get; set; }
    }
}
