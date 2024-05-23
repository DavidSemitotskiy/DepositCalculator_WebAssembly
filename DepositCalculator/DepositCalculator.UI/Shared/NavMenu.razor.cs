using System.Diagnostics.CodeAnalysis;
using DepositCalculator.UI.Interfaces;
using Microsoft.AspNetCore.Components;

namespace DepositCalculator.UI.Shared
{
    /// <summary>
    /// Represents navigation menu component to switch between pages.
    /// </summary>
    public partial class NavMenu
    {
        /// <summary>
        /// Gets or sets the <see cref="IDepositCalculatorApiService" /> to clear current
        /// <see cref="IDepositCalculationStateService.CalculationPageModel" /> while
        /// switching between pages.
        /// </summary>
        [Inject]
        [NotNull]
        public IDepositCalculationStateService DepositCalculationStateService { get; set; }

        private void ClearDepositCalculationStateService()
        {
            DepositCalculationStateService.ClearDepositCalculationPageModel();
        }
    }
}