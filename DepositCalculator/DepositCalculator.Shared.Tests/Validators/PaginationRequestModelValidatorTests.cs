using System.Collections.Generic;
using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Utils;
using DepositCalculator.Shared.Validators;
using FluentValidation.TestHelper;

namespace DepositCalculator.Shared.Tests.Validators
{
    internal class PaginationRequestModelValidatorTests
    {
        private PaginationRequestModelValidator _systemUnderTest;

        [SetUp]
        public void Setup()
        {
            _systemUnderTest = new PaginationRequestModelValidator();
        }

        [TestCaseSource(nameof(ValidPaginationRequestModelTestCases))]
        public void Validate_ValidPaginationRequestModel_ShouldNotBeValidationErrors(PaginationRequestModel paginationRequest)
        {
            // Arrange

            // Act
            TestValidationResult<PaginationRequestModel> result = _systemUnderTest.TestValidate(paginationRequest);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(17)]
        [TestCase(0)]
        public void Validate_PageSizeNotInclusiveBetween1AndMaxPageSize_ShouldBeValidationErrorForPageSize(int pageSize)
        {
            // Arrange
            var fakePaginationRequest = new PaginationRequestModel { PageSize = pageSize };

            string expectedErrorMessage = string.Format(
                PaginationRequestModelValidator.PageSizeNotInclusiveBetween1AndMaxPageSize,
                "Page Size",
                PaginationConstants.MaxPageSize,
                pageSize);

            // Act
            TestValidationResult<PaginationRequestModel> actualResult = _systemUnderTest.TestValidate(fakePaginationRequest);

            // Assert
            actualResult
                .ShouldHaveValidationErrorFor(member => member.PageSize)
                .WithErrorMessage(expectedErrorMessage);
        }

        [TestCase(0)]
        [TestCase(-23)]
        public void Validate_PageNumberLessThan1_ShouldBeValidationErrorForPageNumber(int pageNumber)
        {
            // Arrange
            var fakePaginationRequest = new PaginationRequestModel { PageNumber = pageNumber };

            string expectedErrorMessage = string.Format(
                PaginationRequestModelValidator.PageNumberLessThan1,
                "Page Number",
                pageNumber);

            // Act
            TestValidationResult<PaginationRequestModel> actualResult = _systemUnderTest.TestValidate(fakePaginationRequest);

            // Assert
            actualResult
                .ShouldHaveValidationErrorFor(member => member.PageNumber)
                .WithErrorMessage(expectedErrorMessage);
        }

        private static IEnumerable<PaginationRequestModel> ValidPaginationRequestModelTestCases()
        {
            yield return new PaginationRequestModel
            {
                PageNumber = 1,
                PageSize = 12
            };

            yield return new PaginationRequestModel
            {
                PageNumber = 23,
                PageSize = 16
            };

            yield return new PaginationRequestModel
            {
                PageNumber = 72,
                PageSize = 1
            };
        }
    }
}