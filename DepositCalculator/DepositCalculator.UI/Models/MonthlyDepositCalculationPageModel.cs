using System.Diagnostics.CodeAnalysis;

namespace DepositCalculator.UI.Models
{
    /// <summary>
    /// Represents deposit calculation model by specific month to display on a page.
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
    public record MonthlyDepositCalculationPageModel(
        int monthNumber,
        decimal depositByMonth,
        decimal totalDepositAmount);
}