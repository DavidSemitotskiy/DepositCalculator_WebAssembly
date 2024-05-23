using System;
using System.Threading.Tasks;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.BLL.Interfaces
{
    /// <summary>
    /// A contract for handling operations on deposit.
    /// </summary>
    public interface IDepositCalculatorService
    {
        /// <summary>
        /// Calculates deposit based on <paramref name="depositInfoRequestDTO" />.
        /// </summary>
        /// <param name="depositInfoRequestDTO">
        /// The <see cref="DepositInfoRequestDTO" /> to provide data for calculation.
        /// </param>
        /// <returns>Calculated <see cref="DepositCalculationResponseDTO" />.</returns>
        Task<DepositCalculationResponseDTO> CalculateAsync(DepositInfoRequestDTO depositInfoRequestDTO);

        /// <summary>
        /// Gets paginated deposit calculations based on <paramref name="paginationRequest" />.
        /// </summary>
        /// <param name="paginationRequest">
        /// The <see cref="PaginationRequestModel" /> to provide number of items and requested page.
        /// </param>
        /// <returns>Paginated <see cref="CalculationDetailsResponseDTO" /> by requested page.</returns>
        Task<PaginatedResponseModel<CalculationDetailsResponseDTO>> GetPaginatedCalculationsAsync(
            PaginationRequestModel paginationRequest);

        /// <summary>
        /// Gets a specific calculation based on <paramref name="calculationId" />.
        /// </summary>
        /// <param name="calculationId">
        /// The <see cref="Guid" /> of existed <see cref="DepositCalculationResponseDTO" />.
        /// </param>
        /// <returns>
        /// The <see cref="DepositCalculationResponseDTO" /> by its <see cref="Guid" />.
        /// </returns>
        Task<DepositCalculationResponseDTO?> GetCalculationByIdAsync(Guid calculationId);
    }
}