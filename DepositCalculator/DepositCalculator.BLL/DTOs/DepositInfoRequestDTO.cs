using DepositCalculator.Shared.Enums;
using System.Diagnostics.CodeAnalysis;

namespace DepositCalculator.BLL.DTOs
{
    /// <summary>
    /// Represents the calculation request.
    /// </summary>
    /// <param name="depositAmount">The initial deposit amount.</param>
    /// <param name="periodInMonths">The duration of the deposit in months.</param>
    /// <param name="percent">The interest rate percentage used for deposit calculation.</param>
    /// <param name="calculationMethod">The method used for deposit calculation.</param>
    [ExcludeFromCodeCoverage]
    public record DepositInfoRequestDTO(
        decimal depositAmount,
        int periodInMonths,
        decimal percent,
        DepositCalculationMethod calculationMethod);
}