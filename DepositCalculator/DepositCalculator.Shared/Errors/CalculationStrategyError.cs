namespace DepositCalculator.Shared.Errors
{
    /// <summary>
    /// The calculation strategy error response.
    /// </summary>
    public class CalculationStrategyError
    {
        /// <summary>
        /// Gets the error message associated with the calculation strategy error.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CalculationStrategyError" /> class using
        /// error message.
        /// </summary>
        /// <param name="errorMessage">The error message describing the calculation strategy error.</param>
        public CalculationStrategyError(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }
    }
}