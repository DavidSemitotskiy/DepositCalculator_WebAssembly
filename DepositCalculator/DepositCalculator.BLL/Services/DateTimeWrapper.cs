using System;
using System.Diagnostics.CodeAnalysis;
using DepositCalculator.BLL.Interfaces;

namespace DepositCalculator.BLL.Services
{
    /// <inheritdoc cref="IDateTimeWrapper" />
    [ExcludeFromCodeCoverage]
    public class DateTimeWrapper : IDateTimeWrapper
    {
        /// <inheritdoc />
        public DateTime Now => DateTime.Now;
    }
}