using System;
using System.Threading.Tasks;
using DepositCalculator.UI.Models;

namespace DepositCalculator.UI.Interfaces
{
    /// <summary>
    /// A state service contract for the provision of shared data and notifications
    /// to subscribers in case of data changes.
    /// </summary>
    public interface IDepositCalculationStateService
    {
        /// <summary>
        /// Event triggered when new <see cref="DepositCalculationPageModel" /> instance was set
        /// to this state service.
        /// </summary>
        event Func<Task>? OnChangeAsync;

        /// <summary>
        /// Gets the current <see cref="DepositCalculationPageModel" />.
        /// </summary>
        DepositCalculationPageModel CalculationPageModel { get; }

        /// <summary>
        /// Sets the new instance of <see cref="DepositCalculationPageModel" /> to this state service
        /// and notifies all subscribers.
        /// </summary>
        /// <param name="depositCalculationPageModel">
        /// The new <see cref="DepositCalculationPageModel" /> instance to set.
        /// </param>
        /// <returns>A task that represents the asynchronous set operation.</returns>
        Task SetDepositCalculationPageModelAsync(DepositCalculationPageModel depositCalculationPageModel);

        /// <summary>
        /// Clears current <see cref="DepositCalculationPageModel" /> by setting empty collection of
        /// <see cref="MonthlyDepositCalculationPageModel" /> if it is not empty; otherwise nothing changes.
        /// </summary>
        void ClearDepositCalculationPageModel();
    }
}