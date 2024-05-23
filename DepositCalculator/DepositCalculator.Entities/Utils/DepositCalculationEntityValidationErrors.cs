namespace DepositCalculator.Entities.Utils
{
    /// <summary>
    /// Constant validation messages for <see cref="DepositCalculationEntity" />.
    /// </summary>
    internal static class DepositCalculationEntityValidationErrors
    {
        /// <summary>
        /// The error message for <see cref="DepositCalculationEntity.Percent" /> not being inclusive between 1 and 100.
        /// </summary>
        internal const string PercentNotInclusiveBetween1And100
            = $"'{nameof(DepositCalculationEntity.Percent)}' must be between 1 and 100 inclusively. You entered {{0}}.";

        /// <summary>
        /// The error message for <see cref="DepositCalculationEntity.PeriodInMonths" /> not being inclusive between 1 and 36.
        /// </summary>
        internal const string PeriodNotInclusiveBetween1And36
            = $"'{nameof(DepositCalculationEntity.PeriodInMonths)}' must be between 1 and 36 inclusively. You entered {{0}}.";

        /// <summary>
        /// The error message for <see cref="DepositCalculationEntity.CalculatedAt" /> being empty.
        /// </summary>
        internal const string EmptyCalculatedAt
            = $"'{nameof(DepositCalculationEntity.CalculatedAt)}' must not be empty.";

        /// <summary>
        /// The error message for <see cref="DepositCalculationEntity.DepositAmount" /> not being inclusive between 1 and 1000000.
        /// </summary>
        internal const string DepositAmountNotInclusiveBetween1And1000000
            = $"'{nameof(DepositCalculationEntity.DepositAmount)}' must be between 1 and 1000000 inclusively. You entered {{0}}.";

        /// <summary>
        /// The error message for <see cref="DepositCalculationEntity.MonthlyCalculations" /> count not equal to <see cref="DepositCalculationEntity.PeriodInMonths" />.
        /// </summary>
        internal const string MonthlyCalculationsCountNotEqualPeriodInMonths
            = $"'{nameof(DepositCalculationEntity.MonthlyCalculations)}' must be equal to '{nameof(DepositCalculationEntity.PeriodInMonths)}'. Actual count {{0}}.";

        /// <summary>
        /// The error message for <see cref="DepositCalculationEntity.MonthlyCalculations" /> being null.
        /// </summary>
        internal const string NullMonthlyCalculations
            = $"{nameof(DepositCalculationEntity.MonthlyCalculations)} must not be null.";
    }
}