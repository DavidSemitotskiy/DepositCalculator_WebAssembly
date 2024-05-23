using System;

namespace DepositCalculator.BLL.Exceptions
{
    /// <summary>
    /// The base class for exceptions related to calculation strategies.
    /// </summary>
    public abstract class StrategyException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code associated with the exception.
        /// </summary>
        public abstract int StatusCode { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StrategyException" />
        /// class using error message.
        /// </summary>
        /// <param name="message">
        /// The error message that explains the reason for the exception.
        /// </param>
        public StrategyException(string message) : base(message)
        {
        }
    }
}