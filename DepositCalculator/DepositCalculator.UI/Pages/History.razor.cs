using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Utils;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DepositCalculator.UI.Pages
{
    /// <summary>
    /// Represents the history page for displaying collection of
    /// <see cref="CalculationDetailsPageModel" /> by pages.
    /// </summary>
    public partial class History
    {
        /// <summary>
        /// Gets or sets <see cref="IDepositCalculatorApiService" /> for interacting with
        /// the DepositCalculator Api.
        /// </summary>
        [Inject]
        [NotNull]
        public IDepositCalculatorApiService DepositCalculatorApiService { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IMapper" /> for mapping between response models and page models.
        /// </summary>
        [Inject]
        [NotNull]
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ILogger{History}" /> for logging messages specific to
        /// the <see cref="History"/> page.
        /// </summary>
        [Inject]
        [NotNull]
        public ILogger<History> Logger { get; set; }

        /// <summary>
        /// Gets or sets <see cref="CalculationsHistoryPageModel" /> for obtaining calculations during
        /// the pagination process to display on a page.
        /// </summary>
        public CalculationsHistoryPageModel CalculationsHistory { get; set; }

        /// <summary>
        /// Gets or sets <see cref="PaginationRequestModel" /> for retrieving paginated calculations.
        /// </summary>
        public PaginationRequestModel PaginationRequest { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether there is a next page of calculations.
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// Retrieves first page of calculations.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            PaginationRequest = new PaginationRequestModel
            {
                PageNumber = 1,
                PageSize = PaginationConstants.MaxPageSize
            };

            CalculationsHistory = new CalculationsHistoryPageModel
            {
                DepositCalculations = new List<CalculationDetailsPageModel>()
            };

            await LoadPaginatedCalculationsAsync();
        }

        private async Task LoadPaginatedCalculationsAsync()
        {
            try
            {
                PaginatedResponseModel<CalculationDetailsResponseModel>? calculationsResponse
                    = await DepositCalculatorApiService.GetPaginatedCalculationsAsync(PaginationRequest);

                SetPaginatedCalculationsResponse(calculationsResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

        private void SetPaginatedCalculationsResponse(PaginatedResponseModel<CalculationDetailsResponseModel>? calculationsResponse)
        {
            if (calculationsResponse == null)
            {
                return;
            }

            IReadOnlyCollection<CalculationDetailsPageModel> calculationPageModels = Mapper
                .Map<IReadOnlyCollection<CalculationDetailsPageModel>>(calculationsResponse.items);

            CalculationsHistory.DepositCalculations.AddRange(calculationPageModels);

            HasNextPage = calculationsResponse.totalCount > CalculationsHistory.DepositCalculations.Count;

            if (HasNextPage)
            {
                PaginationRequest.PageNumber++;
            }
        }
    }
}