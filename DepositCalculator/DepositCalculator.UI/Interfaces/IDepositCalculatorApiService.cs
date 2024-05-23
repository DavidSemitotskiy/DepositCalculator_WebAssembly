using System;
using System.Threading.Tasks;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.UI.Interfaces
{
    /// <summary>
    /// A contract for interacting with the DepositCalculator Api.
    /// </summary>
    public interface IDepositCalculatorApiService
    {
        /// <summary>
        /// Sends HTTP POST request to calculate deposit asynchronously
        /// based on the <paramref name="depositInfoViewModel" />.
        /// </summary>
        /// <param name="depositInfoViewModel">
        /// The <see cref="DepositInfoRequestModel" /> to provide data for calculation.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The result is calculated
        /// <see cref="DepositCalculationResponseModel" /> if the response was success; otherwise it is null.
        /// </returns>
        Task<DepositCalculationResponseModel?> CalculateAsync(DepositInfoRequestModel depositInfoViewModel);

        /// <summary>
        /// Sends HTTP GET request to get paginated <see cref="CalculationDetailsResponseModel" />
        /// based on the <paramref name="paginationRequest" />.
        /// </summary>
        /// <param name="paginationRequest">
        /// The <see cref="PaginationRequestModel" /> to provide number of items and requested page.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The result is pginated
        /// <see cref="CalculationDetailsResponseModel" /> if the response was success; otherwise it is null.
        /// </returns>
        Task<PaginatedResponseModel<CalculationDetailsResponseModel>?> GetPaginatedCalculationsAsync(
            PaginationRequestModel paginationRequest);

        /// <summary>
        /// Sends HTTP GET request to get <see cref="DepositCalculationResponseModel" /> based on
        /// <paramref name="calculationId" />.
        /// </summary>
        /// <param name="calculationId">
        /// The <see cref="Guid" /> of existed <see cref="DepositCalculationResponseModel" />.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation. The result is
        /// <see cref="DepositCalculationResponseModel" /> if the response was success;
        /// otherwise it is null.
        /// </returns>
        Task<DepositCalculationResponseModel?> GetCalculationByIdAsync(Guid calculationId);
    }
}