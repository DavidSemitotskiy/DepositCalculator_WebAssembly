namespace DepositCalculator.UI.Utils
{
    /// <summary>
    /// The options of DepositCalculator Api to send requests to.
    /// </summary>
    public class DepositCalculatorApiOptions
    {
        /// <summary>
        /// The section name with options of DepositCalculator Api.
        /// </summary>
        public const string SectionName = "DepositCalculator";

        /// <summary>
        /// Gets or sets base Uri for requests to DepositCalculator Api.
        /// </summary>
        public string BaseUri { get; set; }

        /// <summary>
        /// Gets or sets Calculate endpoint of DepositCalculator Api.
        /// </summary>
        public string CalculateEndpoint { get; set; }

        /// <summary>
        /// Gets or sets Calculations endpoint of DepositCalculator Api.
        /// </summary>
        public string CalculationsEndpoint { get; set; }
    }
}