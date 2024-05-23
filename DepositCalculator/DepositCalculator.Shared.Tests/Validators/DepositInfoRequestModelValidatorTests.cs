using System.Collections.Generic;
using DepositCalculator.Shared.Enums;
using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Validators;
using FluentValidation;
using FluentValidation.TestHelper;

namespace DepositCalculator.Shared.Tests.Validators
{
    internal class DepositInfoRequestModelValidatorTests
    {
        private IValidator<DepositInfoRequestModel> _systemUnderTest;

        [SetUp]
        public void Setup()
        {
            _systemUnderTest = new DepositInfoRequestModelValidator();
        }

        [TestCaseSource(nameof(ValidDepositInfoRequestModelTestCases))]
        public void Validate_ValidDepositInfoRequestModel_ShouldNotBeValidationErrors(
            DepositInfoRequestModel depositInfoRequestModel)
        {
            // Arrange

            // Act
            TestValidationResult<DepositInfoRequestModel> result = _systemUnderTest.TestValidate(depositInfoRequestModel);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [TestCase(0)]
        [TestCase(-12)]
        public void Validate_DepositAmountLessThanOrEqual0_ShouldBeValidationErrorForDepositAmount(decimal depositAmount)
        {
            // Arrange
            var fakeDepositInfoRequestModel = new DepositInfoRequestModel { DepositAmount = depositAmount };

            string expectedErrorMessage = string.Format(
                DepositInfoRequestModelValidator.DepositLessThanZero,
                "Deposit Amount",
                depositAmount);

            // Act
            TestValidationResult<DepositInfoRequestModel> actualResult = _systemUnderTest
                .TestValidate(fakeDepositInfoRequestModel);

            // Assert
            actualResult
                .ShouldHaveValidationErrorFor(member => member.DepositAmount)
                .WithErrorMessage(expectedErrorMessage);
        }

        [Test]
        public void Validate_NullProperties_ShouldBeValidationErrorsForProperties()
        {
            // Arrange
            var fakeDepositInfoRequestModel = new DepositInfoRequestModel();

            string expectedErrorMessage = DepositInfoRequestModelValidator.NullProperty;

            // Act
            TestValidationResult<DepositInfoRequestModel> actualResult = _systemUnderTest
                .TestValidate(fakeDepositInfoRequestModel);

            // Assert
            actualResult
                .ShouldHaveValidationErrorFor(member => member.DepositAmount)
                .WithErrorMessage(string.Format(expectedErrorMessage, "Deposit Amount"));

            actualResult
                .ShouldHaveValidationErrorFor(member => member.PeriodInMonths)
                .WithErrorMessage(string.Format(expectedErrorMessage, "Period In Months"));

            actualResult
                .ShouldHaveValidationErrorFor(member => member.Percent)
                .WithErrorMessage(string.Format(expectedErrorMessage, "Percent"));
        }

        [TestCase(1000230)]
        [TestCase(1235139)]
        public void Validate_DepositAmountGreaterThan1000000_ShouldBeValidationErrorForDepositAmount(decimal depositAmount)
        {
            // Arrange
            var fakeDepositInfoRequestModel = new DepositInfoRequestModel { DepositAmount = depositAmount };

            string expectedErrorMessage = string.Format(
                DepositInfoRequestModelValidator.DepositBiggerThanMillion,
                "Deposit Amount",
                depositAmount);

            // Act
            TestValidationResult<DepositInfoRequestModel> actualResult = _systemUnderTest
                .TestValidate(fakeDepositInfoRequestModel);

            // Assert
            actualResult
                .ShouldHaveValidationErrorFor(member => member.DepositAmount)
                .WithErrorMessage(expectedErrorMessage);
        }

        [TestCase(26.2912)]
        [TestCase(9172.131)]
        public void Validate_DepositAmountHasMoreDecimalsThan2_ShouldBeValidationErrorForDepositAmount(decimal depositAmount)
        {
            // Arrange
            var fakeDepositInfoRequestModel = new DepositInfoRequestModel { DepositAmount = depositAmount };

            string expectedErrorMessage = string.Format(
                DepositInfoRequestModelValidator.DepositHasMoreDecimalsThanTwo,
                "Deposit Amount",
                depositAmount);

            // Act
            TestValidationResult<DepositInfoRequestModel> actualResult = _systemUnderTest
                .TestValidate(fakeDepositInfoRequestModel);

            // Assert
            actualResult
                .ShouldHaveValidationErrorFor(member => member.DepositAmount)
                .WithErrorMessage(expectedErrorMessage);
        }

        [TestCase(-12)]
        [TestCase(0)]
        [TestCase(38)]
        public void Validate_PeriodInMonthsNotInclusiveBetween1And36_ShouldBeValidationErrorForPeriodInMonths(int periodInMonths)
        {
            // Arrange
            var fakeDepositInfoRequestModel = new DepositInfoRequestModel { PeriodInMonths = periodInMonths };

            string expectedErrorMessage = string.Format(
                DepositInfoRequestModelValidator.PeriodIsNotInclusiveBetween1And36,
                "Period In Months",
                periodInMonths);

            // Act
            TestValidationResult<DepositInfoRequestModel> actualResult = _systemUnderTest
                .TestValidate(fakeDepositInfoRequestModel);

            // Assert
            actualResult
                .ShouldHaveValidationErrorFor(member => member.PeriodInMonths)
                .WithErrorMessage(expectedErrorMessage);
        }

        [TestCase(-81.21)]
        [TestCase(0)]
        [TestCase(101.912)]
        public void Validate_PercentNotInclusiveBetween1And100_ShouldBeValidationErrorForPercent(decimal percent)
        {
            // Arrange
            var fakeDepositInfoRequestModel = new DepositInfoRequestModel { Percent = percent };

            string expectedErrorMessage = string.Format(
                DepositInfoRequestModelValidator.PercentIsNotInclusiveBetween1And100,
                "Percent",
                percent);

            // Act
            TestValidationResult<DepositInfoRequestModel> actualResult = _systemUnderTest
                .TestValidate(fakeDepositInfoRequestModel);

            // Assert
            actualResult
                .ShouldHaveValidationErrorFor(member => member.Percent)
                .WithErrorMessage(expectedErrorMessage);
        }

        [TestCase(-1)]
        [TestCase(2)]
        public void Validate_CalculationMethodNotInDepositCalculationMethodEnum_ShouldBeValidationErrorForCalculationMethod(
            DepositCalculationMethod calculationMethod)
        {
            // Arrange
            var fakeDepositInfoRequestModel = new DepositInfoRequestModel { CalculationMethod = calculationMethod };

            string expectedErrorMessage = DepositInfoRequestModelValidator.CalculationMethodNotFound;

            // Act
            TestValidationResult<DepositInfoRequestModel> actualResult = _systemUnderTest
                .TestValidate(fakeDepositInfoRequestModel);

            // Assert
            actualResult
                .ShouldHaveValidationErrorFor(member => member.CalculationMethod)
                .WithErrorMessage(expectedErrorMessage);
        }

        private static IEnumerable<DepositInfoRequestModel> ValidDepositInfoRequestModelTestCases()
        {
            yield return new DepositInfoRequestModel
            {
                DepositAmount = 8123,
                Percent = 23,
                PeriodInMonths = 12,
                CalculationMethod = DepositCalculationMethod.Compound
            };

            yield return new DepositInfoRequestModel
            {
                DepositAmount = 812,
                Percent = 66,
                PeriodInMonths = 1,
                CalculationMethod = DepositCalculationMethod.Simple
            };

            yield return new DepositInfoRequestModel
            {
                DepositAmount = 23913,
                Percent = 8,
                PeriodInMonths = 36,
                CalculationMethod = DepositCalculationMethod.Compound
            };
        }
    }
}