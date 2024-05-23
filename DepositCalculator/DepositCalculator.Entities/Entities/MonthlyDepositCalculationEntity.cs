using System;

namespace DepositCalculator.Entities
{
    /// <summary>
    /// The entity for storing deposit calculation by specific month.
    /// </summary>
    public class MonthlyDepositCalculationEntity : BaseEntity<Guid>
    {
        /// <summary>
        /// Gets or sets the month number for which the calculation is performed.
        /// </summary>
        public int MonthNumber { get; set; }

        /// <summary>
        /// Gets or sets the deposit amount for the specific month.
        /// </summary>
        public decimal DepositByMonth { get; set; }

        /// <summary>
        /// Gets or sets the total deposit amount accumulated up to the specific month.
        /// </summary>
        public decimal TotalDepositAmount { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the corresponding <see cref="DepositCalculationEntity" />.
        /// </summary>
        public Guid DepositCalculationId { get; set; }
    }
}