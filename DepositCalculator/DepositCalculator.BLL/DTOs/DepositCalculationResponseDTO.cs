using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DepositCalculator.BLL.DTOs
{
    /// <summary>
    /// Represents deposit calculation information including collection of
    /// <see cref="MonthlyDepositCalculationResponseDTO" /> by each month of deposit duration.
    /// </summary>
    /// <param name="monthlyCalculations">
    /// The collection of <see cref="MonthlyDepositCalculationResponseDTO" /> by each month
    /// of deposit duration.
    /// </param>
    [ExcludeFromCodeCoverage]
    public record DepositCalculationResponseDTO(
        IReadOnlyCollection<MonthlyDepositCalculationResponseDTO> monthlyCalculations);
}