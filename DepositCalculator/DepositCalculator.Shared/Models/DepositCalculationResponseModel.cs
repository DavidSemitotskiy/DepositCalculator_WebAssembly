using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DepositCalculator.Shared.Models
{
    /// <summary>
    /// Represents a response model for deposit calculation information including the collection
    /// of <see cref="MonthlyDepositCalculationResponseModel" /> for each month of the deposit duration.
    /// </summary>
    /// <param name="monthlyCalculations">
    /// The collection of <see cref="MonthlyDepositCalculationResponseModel" /> by each month
    /// of deposit duration.
    /// </param>
    [ExcludeFromCodeCoverage]
    public record DepositCalculationResponseModel(
        IReadOnlyCollection<MonthlyDepositCalculationResponseModel> monthlyCalculations);
}