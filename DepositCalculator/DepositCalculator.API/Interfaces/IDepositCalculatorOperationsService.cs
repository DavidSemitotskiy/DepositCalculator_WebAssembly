using System;
using System.Threading.Tasks;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.API.Interfaces
{
    /// <summary>
    /// A contract for mapping between DTOs and response models.
    /// </summary>
    public interface IDepositCalculatorOperationsService
    {
        /// <summary>
        /// Maps <see cref="DepositInfoRequestModel" /> to <see cref="DepositInfoRequestDTO" /> for passing data
        /// to Business Logic Layer and maps <see cref="DepositCalculationResponseDTO" /><br/> to
        /// <see cref="DepositCalculationResponseModel" /> for returning calculated data to Presentation Layer.
        /// </summary>
        /// <param name="depositInfoRequest">
        /// The <see cref="DepositInfoRequestModel" /> that provides data for calculation.
        /// </param>
        /// <returns>
        /// The <see cref="DepositCalculationResponseModel" /> mapped from <see cref="DepositCalculationResponseDTO" />.
        /// </returns>
        Task<DepositCalculationResponseModel> CalculateAsync(DepositInfoRequestModel depositInfoRequest);

        /// <summary>
        /// Maps paginated <see cref="CalculationDetailsResponseDTO" /> to paginated
        /// <see cref="CalculationDetailsResponseModel" /> for returning paginated calculations to Presentation Layer.
        /// </summary>
        /// <param name="paginationRequest">
        /// The <see cref="PaginationRequestModel" /> that provides number of items and requested page.</param>
        /// <returns>
        /// The paginated <see cref="CalculationDetailsResponseModel" /> mapped from paginated
        /// <see cref="CalculationDetailsResponseDTO" />.
        /// </returns>
        Task<PaginatedResponseModel<CalculationDetailsResponseModel>> GetPaginatedCalculationsAsync(
            PaginationRequestModel paginationRequest);

        /// <summary>
        /// Maps <see cref="DepositCalculationResponseDTO" /> to <see cref="DepositCalculationResponseModel" />
        /// for returning calculation data to Presentation Layer based on <paramref name="calculationId" />.
        /// </summary>
        /// <param name="calculationId">
        /// The <see cref="Guid" /> of existed <see cref="DepositCalculationResponseModel" />.
        /// </param>
        /// <returns>
        /// The <see cref="DepositCalculationResponseModel" /> mapped from <see cref="DepositCalculationResponseDTO" />.
        /// </returns>
        Task<DepositCalculationResponseModel?> GetCalculationByIdAsync(Guid calculationId);
    }
}