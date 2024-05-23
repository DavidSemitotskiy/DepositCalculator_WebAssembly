using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Utils;
using FluentValidation;

namespace DepositCalculator.Shared.Validators
{
    /// <summary>
    /// Validates instances of <see cref="PaginationRequestModel"/>.
    /// </summary>
    public class PaginationRequestModelValidator : AbstractValidator<PaginationRequestModel>
    {
        /// <summary>
        /// The error message for <see cref="PaginationRequestModel.PageSize" /> not being inclusive between 1 and
        /// <see cref="PaginationConstants.MaxPageSize" />.
        /// </summary>
        internal const string PageSizeNotInclusiveBetween1AndMaxPageSize = "'{0}' must be between 1 and {1} inclusively. You entered {2}.";

        /// <summary>
        /// The error message for <see cref="PaginationRequestModel.PageNumber" /> being less than 1.
        /// </summary>
        internal const string PageNumberLessThan1 = "'{0}' must be greater than or equal to 1. You entered {1}.";

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationRequestModelValidator" /> class and
        /// defines rules for <see cref="PaginationRequestModel" /> properties.
        /// </summary>
        public PaginationRequestModelValidator()
        {
            RuleFor(paginationRequest => paginationRequest.PageSize)
                .InclusiveBetween(1, PaginationConstants.MaxPageSize)
                .WithMessage(string.Format(
                    PageSizeNotInclusiveBetween1AndMaxPageSize,
                    "{PropertyName}",
                    PaginationConstants.MaxPageSize,
                    "{PropertyValue}"));

            RuleFor(paginationRequest => paginationRequest.PageNumber)
                .GreaterThanOrEqualTo(1)
                .WithMessage(string.Format(PageNumberLessThan1, "{PropertyName}", "{PropertyValue}"));
        }
    }
}