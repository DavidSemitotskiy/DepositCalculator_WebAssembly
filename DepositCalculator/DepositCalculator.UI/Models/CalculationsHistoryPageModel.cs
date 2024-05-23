using System.Collections.Generic;

namespace DepositCalculator.UI.Models
{
    /// <summary>
    /// Represents the model with a history of calculations obtained during the pagination
    /// process to display on a page.
    /// </summary>
    public class CalculationsHistoryPageModel
    {
        /// <summary>
        /// Gets or sets the list of <see cref="CalculationDetailsPageModel" /> obtained
        /// during the pagination.
        /// </summary>
        public List<CalculationDetailsPageModel> DepositCalculations { get; set; }
    }
}