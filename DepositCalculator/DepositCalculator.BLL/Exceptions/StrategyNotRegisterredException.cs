using DepositCalculator.BLL.Interfaces;

namespace DepositCalculator.BLL.Exceptions
{
    /// <summary>
    /// Represents an exception thrown when a calculation strategy was not provided for
    /// <see cref="IDepositCalculationStrategyFactory" />.
    /// </summary>
    public class StrategyNotRegisterredException : StrategyException
    {
        /// <inheritdoc />
        public override int StatusCode => 500;

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyNotRegisterredException" />
        /// class using error message.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public StrategyNotRegisterredException(string message) : base(message)
        {
        }
    }
}