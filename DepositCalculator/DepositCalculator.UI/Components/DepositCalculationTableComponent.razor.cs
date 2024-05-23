using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Models;
using DepositCalculator.UI.Utils;
using Microsoft.AspNetCore.Components;

namespace DepositCalculator.UI.Components
{
    /// <summary>
    /// Represents the component for displaying calculated
    /// <see cref="DepositCalculationPageModel" /> as a table.
    /// </summary>
    public partial class DepositCalculationTableComponent : IDisposable
    {
        /// <summary>
        /// The number of calculations required to enable scrolling in the table.
        /// </summary>
        public const int CalculationsCountForScroll = 13;

        /// <summary>
        /// Gets or sets <see cref="IDepositCalculationStateService" /> for retrieving calculated
        /// <see cref="DepositCalculationPageModel" />.
        /// </summary>
        [Inject]
        [NotNull]
        public IDepositCalculationStateService DepositCalculationStateService { get; set; }

        /// <summary>
        /// Gets or sets <see cref="DepositCalculationPageModel" /> to display in the table.
        /// </summary>
        public DepositCalculationPageModel DepositCalculation { get; set; }

        /// <summary>
        /// Unsubscribes from <see cref="IDepositCalculationStateService.OnChangeAsync" /> event.
        /// </summary>
        public void Dispose()
        {
            DepositCalculationStateService.OnChangeAsync -= CalculationsChangedAsync;
        }

        /// <summary>
        /// Method invoked when the component is ready to start, having received its
        /// initial parameters from its parent in the render tree.
        /// Subscibes on <see cref="IDepositCalculationStateService.OnChangeAsync" /> event and rounds calculations.
        /// </summary>
        protected override void OnInitialized()
        {
            DepositCalculationStateService.OnChangeAsync += CalculationsChangedAsync;
            RoundCalculations();
        }

        private async Task CalculationsChangedAsync()
        {
            RoundCalculations();
            await InvokeAsync(StateHasChanged);
        }

        private void RoundCalculations()
        {
            var roundedCalculations = DepositCalculationStateService.CalculationPageModel.monthlyCalculations
                .Select(calculation => new MonthlyDepositCalculationPageModel(
                    calculation.monthNumber,
                    Math.Round(calculation.depositByMonth, 2),
                    Math.Round(calculation.totalDepositAmount, 2)))
                .ToList();

            DepositCalculation = new DepositCalculationPageModel(roundedCalculations);
        }

        private string GetHeaderOffset()
        {
            return IsCalculationTableScrollable() ? DepositTableStyles.TableHeaderWithOffset
                : DepositTableStyles.TableHeaderWithoutOffset;
        }

        private string GetCalculationsTableStyle()
        {
            return IsCalculationTableScrollable() ? DepositTableStyles.CalculationsTableWithScroll : string.Empty;
        }

        private bool IsCalculationTableScrollable()
        {
            return DepositCalculation.monthlyCalculations.Count >= CalculationsCountForScroll;
        }
    }
}