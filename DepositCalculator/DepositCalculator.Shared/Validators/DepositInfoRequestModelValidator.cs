using DepositCalculator.Shared.Enums;
using DepositCalculator.Shared.Models;
using FluentValidation;

namespace DepositCalculator.Shared.Validators
{
    /// <summary>
    /// Validates instances of <see cref="DepositInfoRequestModel"/>.
    /// </summary>
    public class DepositInfoRequestModelValidator : AbstractValidator<DepositInfoRequestModel>
    {
        /// <summary>
        /// The error message for <see cref="DepositInfoRequestModel" /> property being null.
        /// </summary>
        internal const string NullProperty = "'{0}' can not be empty.";

        /// <summary>
        /// The error message for <see cref="DepositInfoRequestModel.DepositAmount" /> being less than 0.
        /// </summary>
        internal const string DepositLessThanZero = "'{0}' must not be negative or equal to zero. You entered {1}.";

        /// <summary>
        /// The error message for <see cref="DepositInfoRequestModel.DepositAmount" /> being bigger than 1000000.
        /// </summary>
        internal const string DepositBiggerThanMillion = "'{0}' can not be more than 1000000. You entered {1}.";

        /// <summary>
        /// The error message for <see cref="DepositInfoRequestModel.DepositAmount" /> having more than 2 decimal places.
        /// </summary>
        internal const string DepositHasMoreDecimalsThanTwo = "'{0}' must not have more than 2 decimals. You entered {1}.";

        /// <summary>
        /// The error message for <see cref="DepositInfoRequestModel.PeriodInMonths" /> being not inclusive between 1 and 36.
        /// </summary>
        internal const string PeriodIsNotInclusiveBetween1And36 = "'{0}' must be between 1 and 36 inclusively. You entered {1}.";

        /// <summary>
        /// The error message for <see cref="DepositInfoRequestModel.Percent" /> being not inclusive between 1 and 100.
        /// </summary>
        internal const string PercentIsNotInclusiveBetween1And100 = "'{0}' must be between 1 and 100 inclusively. You entered {1}.";

        /// <summary>
        /// The error message for <see cref="DepositInfoRequestModel.CalculationMethod" /> being not in <see cref="DepositCalculationMethod" />.
        /// </summary>
        internal const string CalculationMethodNotFound = "Calculation method does not exist.";

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositInfoRequestModelValidator" /> class and
        /// defines rules for <see cref="DepositInfoRequestModel" /> properties.
        /// </summary>
        public DepositInfoRequestModelValidator()
        {
            RuleFor(depositInfoRequestModel => depositInfoRequestModel.DepositAmount)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(string.Format(NullProperty, "{PropertyName}"))
                .GreaterThan(0)
                .WithMessage(string.Format(DepositLessThanZero, "{PropertyName}", "{PropertyValue}"))
                .LessThanOrEqualTo(1000000)
                .WithMessage(string.Format(DepositBiggerThanMillion, "{PropertyName}", "{PropertyValue}"))
                .PrecisionScale(9, 2, ignoreTrailingZeros: false)
                .WithMessage(string.Format(DepositHasMoreDecimalsThanTwo, "{PropertyName}", "{PropertyValue}"));

            RuleFor(depositInfoRequestModel => depositInfoRequestModel.PeriodInMonths)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(string.Format(NullProperty, "{PropertyName}"))
                .InclusiveBetween(1, 36)
                .WithMessage(string.Format(PeriodIsNotInclusiveBetween1And36, "{PropertyName}", "{PropertyValue}"));

            RuleFor(depositInfoRequestModel => depositInfoRequestModel.Percent)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .WithMessage(string.Format(NullProperty, "{PropertyName}"))
                .InclusiveBetween(1, 100)
                .WithMessage(string.Format(PercentIsNotInclusiveBetween1And100, "{PropertyName}", "{PropertyValue}"));

            RuleFor(depositInfoRequestModel => depositInfoRequestModel.CalculationMethod)
                .IsInEnum()
                .WithMessage(CalculationMethodNotFound);
        }
    }
}