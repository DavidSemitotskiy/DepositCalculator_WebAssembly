namespace DepositCalculator.API.Utils
{
    /// <summary>
    /// The options for cross-origin resource sharing services.
    /// </summary>
    public class ApiCorsOptions
    {
        /// <summary>
        /// The section name with options for cross-origin
        /// resource sharing services.
        /// </summary>
        public const string SectionName = "CORS";

        /// <summary>
        /// Gets or sets allowed origins.
        /// </summary>
        public string[] AllowedOrigins { get; set; }
    }
}