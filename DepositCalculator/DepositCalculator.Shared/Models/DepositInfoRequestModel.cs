using DepositCalculator.Shared.Enums;

namespace DepositCalculator.Shared.Models
{
    /// <summary>
    /// Represents the calculation request.
    /// </summary>
    public class DepositInfoRequestModel
    {
        /// <summary>
        /// Gets or sets the initial deposit amount.
        /// </summary>
        public decimal? DepositAmount { get; set; }

        /// <summary>
        /// Gets or sets the duration of the deposit in months.
        /// </summary>
        public int? PeriodInMonths { get; set; }

        /// <summary>
        /// Gets or sets the interest rate percentage used for deposit calculation.
        /// </summary>
        public decimal? Percent { get; set; }

        /// <summary>
        /// Gets or sets the method used for deposit calculation.
        /// </summary>
        public DepositCalculationMethod CalculationMethod { get; set; }
    }
}