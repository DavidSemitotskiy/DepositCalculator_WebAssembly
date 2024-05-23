using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Models;

namespace DepositCalculator.UI.Services
{
    /// <inheritdoc cref="IDepositCalculationStateService" />
    public class DepositCalculationStateService : IDepositCalculationStateService
    {
        /// <inheritdoc />
        public DepositCalculationPageModel CalculationPageModel { get; private set; }
            = new DepositCalculationPageModel(new List<MonthlyDepositCalculationPageModel>());

        /// <inheritdoc />
        public event Func<Task>? OnChangeAsync;

        /// <inheritdoc />
        public void ClearDepositCalculationPageModel()
        {
            if (CalculationPageModel.monthlyCalculations.Count == 0)
            {
                return;
            }

            CalculationPageModel = new DepositCalculationPageModel(new List<MonthlyDepositCalculationPageModel>());
        }

        /// <inheritdoc />
        public async Task SetDepositCalculationPageModelAsync(DepositCalculationPageModel depositCalculationPageModel)
        {
            CalculationPageModel = depositCalculationPageModel;
            await NotifyStateChangedAsync();
        }

        private async Task NotifyStateChangedAsync()
        {
            if (OnChangeAsync is null)
            {
                return;
            }

            await OnChangeAsync();
        }
    }
}