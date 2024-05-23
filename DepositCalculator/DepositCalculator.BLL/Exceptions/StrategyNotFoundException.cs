using DepositCalculator.BLL.Interfaces;

namespace DepositCalculator.BLL.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when a calculation strategy is not found in
    /// specific <see cref="IDepositCalculationStrategyResolver" />.
    /// </summary>
    public class StrategyNotFoundException : StrategyException
    {
        /// <inheritdoc />
        public override int StatusCode => 404;

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyNotFoundException" />
        /// class using error message.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public StrategyNotFoundException(string message) : base(message)
        {
        }
    }
}