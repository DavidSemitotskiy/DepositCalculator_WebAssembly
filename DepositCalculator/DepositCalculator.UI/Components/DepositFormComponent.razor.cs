using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using DepositCalculator.Shared.Models;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DepositCalculator.UI.Components
{
    /// <summary>
    /// Represents the component for handling form submissions to calculate deposit.
    /// </summary>
    public partial class DepositFormComponent
    {
        /// <summary>
        /// Gets or sets <see cref="IMapper" /> for mapping between response models and page models.
        /// </summary>
        [Inject]
        [NotNull]
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IDepositCalculatorApiService" /> for interacting with
        /// the DepositCalculator Api.
        /// </summary>
        [Inject]
        [NotNull]
        public IDepositCalculatorApiService DepositCalculatorApiService { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IDepositCalculationStateService" /> for setting calculated
        /// <see cref="DepositCalculationPageModel" /> to share with dependent services.
        /// </summary>
        [Inject]
        [NotNull]
        public IDepositCalculationStateService DepositCalculationStateService { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ILogger{DepositFormComponent}" /> for logging messages specific to
        /// the <see cref="DepositFormComponent" />.
        /// </summary>
        [Inject]
        [NotNull]
        public ILogger<DepositFormComponent> Logger { get; set; }

        /// <summary>
        /// Gets or sets <see cref="DepositInfoRequestModel" /> containing data for performing calculation.
        /// </summary>
        public DepositInfoRequestModel DepositInfoRequest { get; set; } = new DepositInfoRequestModel();

        private async Task SubmitDepositInfoAsync()
        {
            try
            {
                DepositCalculationResponseModel? calculationResponse = await DepositCalculatorApiService
                    .CalculateAsync(DepositInfoRequest);

                await SetCalculationToStateServiceAsync(calculationResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

        private async Task SetCalculationToStateServiceAsync(DepositCalculationResponseModel? calculationResponse)
        {
            if (calculationResponse == null)
            {
                return;
            }

            DepositCalculationPageModel calculationPageModel = Mapper
                .Map<DepositCalculationPageModel>(calculationResponse);

            await DepositCalculationStateService
                .SetDepositCalculationPageModelAsync(calculationPageModel);
        }
    }
}