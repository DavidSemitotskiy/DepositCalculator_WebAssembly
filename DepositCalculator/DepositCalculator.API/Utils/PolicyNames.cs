namespace DepositCalculator.API.Utils
{
    /// <summary>
    /// The policy names for cross-origin resource sharing services.
    /// </summary>
    public static class PolicyNames
    {
        /// <summary>
        /// The name of policy that allows only specific origins, any headers and any HTTP methods.
        /// </summary>
        public const string SpecificOrigins = "SpecificOriginsCors";

        /// <summary>
        /// The name of policy that allows only specific origins, specific pagination headers and HTTP GET method.
        /// </summary>
        public const string SpecificPaginationResponseHeaders = "SpecificPaginationResponseHeadersCors";
    }
}