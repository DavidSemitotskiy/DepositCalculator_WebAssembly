using System.Diagnostics.CodeAnalysis;

namespace DepositCalculator.BLL.DTOs
{
    /// <summary>
    /// Represents deposit calculation by specific month.
    /// </summary>
    /// <param name="monthNumber">
    /// The month number for which the calculation is performed.
    /// </param>
    /// <param name="depositByMonth">
    /// The deposit amount for the specific month.
    /// </param>
    /// <param name="totalDepositAmount">
    /// The total deposit amount accumulated up to the specific month.
    /// </param>
    [ExcludeFromCodeCoverage]
    public record MonthlyDepositCalculationResponseDTO(
        int monthNumber,
        decimal depositByMonth,
        decimal totalDepositAmount);
}