using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DepositCalculator.Entities.Utils;

namespace DepositCalculator.Entities
{
    /// <summary>
    /// The entity for storing deposit calculation information including collection of
    /// <see cref="MonthlyDepositCalculationEntity" /> by each month of deposit duration.
    /// </summary>
    public class DepositCalculationEntity : BaseEntity<Guid>, IValidatableObject
    {
        /// <summary>
        /// Gets or sets the interest rate percentage used in the calculation.
        /// </summary>
        public decimal Percent { get; set; }

        /// <summary>
        /// Gets or sets the duration of the deposit in months.
        /// </summary>
        public int PeriodInMonths { get; set; }

        /// <summary>
        /// Gets or sets the initial deposit amount.
        /// </summary>
        public decimal DepositAmount { get; set; }

        /// <summary>
        /// Gets or sets the date when deposit was calculated.
        /// </summary>
        public DateTime CalculatedAt { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="MonthlyDepositCalculationEntity" /> by each month
        /// of deposit duration.
        /// </summary>
        public ICollection<MonthlyDepositCalculationEntity> MonthlyCalculations { get; set; }

        /// <inheritdoc />
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Percent < 1 || Percent > 100)
            {
                yield return new ValidationResult(string.Format(
                    DepositCalculationEntityValidationErrors.PercentNotInclusiveBetween1And100,
                    Percent));
            }

            if (PeriodInMonths < 1 || PeriodInMonths > 36)
            {
                yield return new ValidationResult(string.Format(
                    DepositCalculationEntityValidationErrors.PeriodNotInclusiveBetween1And36,
                    PeriodInMonths));
            }

            if (DepositAmount < 1 || DepositAmount > 1000000)
            {
                yield return new ValidationResult(string.Format(
                    DepositCalculationEntityValidationErrors.DepositAmountNotInclusiveBetween1And1000000,
                    DepositAmount));
            }

            if (CalculatedAt == DateTime.MinValue)
            {
                yield return new ValidationResult(DepositCalculationEntityValidationErrors.EmptyCalculatedAt);
            }

            if (MonthlyCalculations == null)
            {
                yield return new ValidationResult(DepositCalculationEntityValidationErrors.NullMonthlyCalculations);

                yield break;
            }

            if (MonthlyCalculations.Count != PeriodInMonths)
            {
                yield return new ValidationResult(string.Format(
                    DepositCalculationEntityValidationErrors.MonthlyCalculationsCountNotEqualPeriodInMonths,
                    MonthlyCalculations.Count));
            }
        }
    }
}