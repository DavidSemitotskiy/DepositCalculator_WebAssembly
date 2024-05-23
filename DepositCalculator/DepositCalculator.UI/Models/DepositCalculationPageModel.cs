using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DepositCalculator.UI.Models
{
    /// <summary>
    /// Represents a model for deposit calculation information including the collection
    /// of <see cref="MonthlyDepositCalculationPageModel" /> for each month of the deposit duration
    /// to display on a page.
    /// </summary>
    /// <param name="monthlyCalculations">
    /// The collection of <see cref="MonthlyDepositCalculationPageModel" /> by each month
    /// of deposit duration.
    /// </param>
    [ExcludeFromCodeCoverage]
    public record DepositCalculationPageModel(
        IReadOnlyCollection<MonthlyDepositCalculationPageModel> monthlyCalculations);
}