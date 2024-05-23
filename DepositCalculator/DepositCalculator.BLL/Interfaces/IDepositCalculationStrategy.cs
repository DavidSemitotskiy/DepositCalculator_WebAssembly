using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Interfaces
{
    /// <summary>
    /// A contract for calculating deposit.
    /// </summary>
    public interface IDepositCalculationStrategy
    {
        /// <summary>
        /// Gets the calculation method associated with the calculation strategy.
        /// </summary>
        DepositCalculationMethod CalculationMethod { get; }

        /// <summary>
        /// Calculates deposit based on <paramref name="depositInfoRequestDTO" />.
        /// </summary>
        /// <param name="depositInfoRequestDTO">
        /// The <see cref="DepositInfoRequestDTO" /> that provides data for calculation.</param>
        /// <returns>Calculated <see cref="DepositCalculationResponseDTO" />.</returns>
        DepositCalculationResponseDTO Calculate(DepositInfoRequestDTO depositInfoRequestDTO);
    }
}