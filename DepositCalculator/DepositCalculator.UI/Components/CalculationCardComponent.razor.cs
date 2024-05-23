using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using AutoMapper;
using Blazored.Modal;
using Blazored.Modal.Services;
using DepositCalculator.Shared.Models;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace DepositCalculator.UI.Components
{
    /// <summary>
    /// Represents component for displaying <see cref="CalculationDetailsPageModel" /> as a card.
    /// </summary>
    public partial class CalculationCardComponent
    {
        /// <summary>
        /// The date format for displaying <see cref="DateTime"/> in the card.
        /// </summary>
        public const string DateFormat = "{0:dd.MM.yyyy}";

        /// <summary>
        /// The rounding format for displaying numeric values in the card.
        /// </summary>
        public const string RoundingFormat = "{0:F0}";

        /// <summary>
        /// Gets or sets the <see cref="CalculationDetailsPageModel" /> to display in the card.
        /// </summary>
        [Parameter]
        [EditorRequired]
        public CalculationDetailsPageModel CalculationDetails { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IModalService" /> for displaying the <see cref="DepositCalculationPageModel" />
        /// retrieved from the DepositCalculator Api based on <see cref="CalculationDetails" /> in a modal window.
        /// </summary>
        [CascadingParameter]
        public IModalService ModalService { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IMapper" /> for mapping between response models and page models.
        /// </summary>
        [Inject]
        [NotNull]
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Gets or sets <see cref="ILogger{CalculationCardComponent}" /> for logging messages specific to
        /// the <see cref="CalculationCardComponent"/>.
        /// </summary>
        [Inject]
        [NotNull]
        public ILogger<CalculationCardComponent> Logger { get; set; }

        /// <summary>
        /// Gets or sets <see cref="IDepositCalculatorApiService" /> for interacting with
        /// the DepositCalculator Api.
        /// </summary>
        [Inject]
        [NotNull]
        public IDepositCalculatorApiService DepositCalculatorApiService { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IDepositCalculationStateService" /> for setting retrieved
        /// <see cref="DepositCalculationPageModel" /> from DepositCalculator Api to share with dependent services.
        /// </summary>
        [Inject]
        [NotNull]
        public IDepositCalculationStateService DepositCalculationStateService { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="ModalOptions" /> for configuring the display of the modal window.
        /// </summary>
        public ModalOptions HistoryDetailsModalOptions { get; set; }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree. Sets default values for properties.
        /// </summary>
        protected override void OnInitialized()
        {
            HistoryDetailsModalOptions = new ModalOptions
            {
                Position = ModalPosition.Middle,
                Size = ModalSize.ExtraLarge
            };
        }

        private async Task LoadDepositCalculationAsync()
        {
            try
            {
                DepositCalculationResponseModel? calculationResponse = await DepositCalculatorApiService
                    .GetCalculationByIdAsync(CalculationDetails.id);

                await ShowHistoryDetailsModal(calculationResponse);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }

        private async Task ShowHistoryDetailsModal(DepositCalculationResponseModel? calculationResponse)
        {
            if (calculationResponse == null)
            {
                return;
            }

            DepositCalculationPageModel calculationPageModel = Mapper
                .Map<DepositCalculationPageModel>(calculationResponse);

            await DepositCalculationStateService.SetDepositCalculationPageModelAsync(calculationPageModel);

            ModalService.Show<HistoryDetailsModalComponent>(string.Empty, HistoryDetailsModalOptions);
        }

        private string FormatPercent(decimal percent)
        {
            return $"{string.Format(RoundingFormat, percent)}%";
        }

        private string FormatDate(DateTime dateTime)
        {
            return string.Format(DateFormat, dateTime);
        }

        private string FormatDepositAmount(decimal depositAmount)
        {
            return $"{string.Format(RoundingFormat, depositAmount)}$";
        }
    }
}