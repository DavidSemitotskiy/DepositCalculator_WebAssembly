using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Utils;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Utils;
using Microsoft.Extensions.Options;

namespace DepositCalculator.UI.Services
{
    /// <inheritdoc cref="IDepositCalculatorApiService" />
    public class DepositCalculatorApiService : IDepositCalculatorApiService
    {
        private readonly IDepositCalculatorHttpClientWrapper _depositCalculatorHttpClientWrapper;

        private readonly DepositCalculatorApiOptions _depositCalculatorApiOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculatorApiService" /> class
        /// using <see cref="IDepositCalculatorHttpClientWrapper" /> and
        /// <see cref="IOptions{DepositCalculatorApiOptions}>" /> of
        /// <see cref="DepositCalculatorApiOptions" />.
        /// </summary>
        /// <param name="depositCalculatorHttpClientWrapper">
        /// The <see cref="IDepositCalculatorHttpClientWrapper" /> for sending request to
        /// DepositCalculator Api.
        /// </param>
        /// <param name="depositCalculatorApiOptions">
        /// The <see cref="IOptions{DepositCalculatorApiOptions}>" /> parametrized by
        /// <see cref="DepositCalculatorApiOptions" /> of DepositCalculator Api to send requests to.
        /// </param>
        public DepositCalculatorApiService(
            IDepositCalculatorHttpClientWrapper depositCalculatorHttpClientWrapper,
            IOptions<DepositCalculatorApiOptions> depositCalculatorApiOptions)
        {
            _depositCalculatorHttpClientWrapper = depositCalculatorHttpClientWrapper;
            _depositCalculatorApiOptions = depositCalculatorApiOptions.Value;
        }

        /// <inheritdoc />
        public async Task<DepositCalculationResponseModel?> CalculateAsync(DepositInfoRequestModel depositInfoRequestModel)
        {
            HttpResponseMessage response = await _depositCalculatorHttpClientWrapper
                .PostAsync(_depositCalculatorApiOptions.CalculateEndpoint, depositInfoRequestModel);

            return await HandleResponse<DepositCalculationResponseModel>(response);
        }

        /// <inheritdoc />
        public async Task<PaginatedResponseModel<CalculationDetailsResponseModel>?> GetPaginatedCalculationsAsync(
            PaginationRequestModel paginationRequest)
        {
            string paginationQueryParameters = string.Format(
                DepositRouteQueryConstants.PaginationQueryParameters,
                paginationRequest.PageSize,
                paginationRequest.PageNumber);

            var requestUri = $"{_depositCalculatorApiOptions.CalculationsEndpoint}?{paginationQueryParameters}";

            HttpResponseMessage response = await _depositCalculatorHttpClientWrapper.GetAsync(requestUri);

            IReadOnlyCollection<CalculationDetailsResponseModel>? calculations
                = await HandleResponse<IReadOnlyCollection<CalculationDetailsResponseModel>>(response);

            if (calculations == null)
            {
                return null;
            }

            IEnumerable<string> headers = response.Headers.GetValues(ResponseHeaderConstants.TotalCount);

            int totalCount = int.Parse(headers.First());

            return new PaginatedResponseModel<CalculationDetailsResponseModel>(
                totalCount,
                calculations);
        }

        /// <inheritdoc />
        public async Task<DepositCalculationResponseModel?> GetCalculationByIdAsync(Guid calculationId)
        {
            var requestUri = $"{_depositCalculatorApiOptions.CalculationsEndpoint}/{calculationId}";

            HttpResponseMessage response = await _depositCalculatorHttpClientWrapper.GetAsync(requestUri);

            return await HandleResponse<DepositCalculationResponseModel>(response);
        }

        private async Task<T?> HandleResponse<T>(HttpResponseMessage response) where T : class
        {
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            return await response.Content.ReadFromJsonAsync<T>();
        }
    }
}