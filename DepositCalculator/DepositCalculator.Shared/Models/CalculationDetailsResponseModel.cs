using System;
using System.Diagnostics.CodeAnalysis;

namespace DepositCalculator.Shared.Models
{
    /// <summary>
    /// Represents the details response model of a deposit calculation itself.
    /// </summary>
    /// <param name="id">The unique identifier for the calculation details.</param>
    /// <param name="percent">The interest rate percentage used in the calculation.</param>
    /// <param name="periodInMonths">The duration of the deposit in months.</param>
    /// <param name="depositAmount">The initial deposit amount.</param>
    /// <param name="calculatedAt">The date when deposit was calculated.</param>
    [ExcludeFromCodeCoverage]
    public record CalculationDetailsResponseModel(
        Guid id,
        decimal percent,
        int periodInMonths,
        decimal depositAmount,
        DateTime calculatedAt);
}