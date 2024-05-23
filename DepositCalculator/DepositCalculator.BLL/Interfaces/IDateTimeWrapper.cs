using System;

namespace DepositCalculator.BLL.Interfaces
{
    /// <summary>
    /// A contract for a wrapper around the <see cref="DateTime" />.
    /// </summary>
    public interface IDateTimeWrapper
    {
        /// <summary>
        /// Gets the current date and time.
        /// </summary>
        DateTime Now { get; }
    }
}