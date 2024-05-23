using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DepositCalculator.Entities.Utils;
using FluentAssertions;

namespace DepositCalculator.Entities.Tests.Entities
{
    internal class DepositCalculationEntityTests
    {
        [TestCaseSource(nameof(ValidDepositCalculationEntityTestCases))]
        public void Validate_ValidDepositCalculationEntity_ShouldNotBeValidationResults(DepositCalculationEntity systemUnderTest)
        {
            // Arrange
            var validationContext = new ValidationContext(systemUnderTest);

            // Act
            IEnumerable<ValidationResult> result = systemUnderTest.Validate(validationContext);

            // Assert
            result
                .Should()
                .BeEmpty();
        }

        [TestCase(0)]
        [TestCase(101)]
        public void Validate_PercentNotInclusiveBetween1And100_ShouldBeValidationResultWithExpectedErrorMessage(decimal percent)
        {
            // Arrange
            var systemUnderTest = new DepositCalculationEntity { Percent = percent };

            var validationContext = new ValidationContext(systemUnderTest);

            string expectedErrorMessage = string.Format(
                DepositCalculationEntityValidationErrors.PercentNotInclusiveBetween1And100,
                percent);

            // Act
            IEnumerable<ValidationResult> actualResult = systemUnderTest.Validate(validationContext);

            // Assert
            ContainsErrorMessage(actualResult, expectedErrorMessage);
        }

        [TestCase(0)]
        [TestCase(37)]
        public void Validate_PeriodInMonthsNotInclusiveBetween1And36_ShouldBeValidationResultWithExpectedErrorMessage(
            int periodInMonths)
        {
            // Arrange
            var systemUnderTest = new DepositCalculationEntity { PeriodInMonths = periodInMonths };

            var validationContext = new ValidationContext(systemUnderTest);

            string expectedErrorMessage = string.Format(
                DepositCalculationEntityValidationErrors.PeriodNotInclusiveBetween1And36,
                periodInMonths);

            // Act
            IEnumerable<ValidationResult> actualResult = systemUnderTest.Validate(validationContext);

            // Assert
            ContainsErrorMessage(actualResult, expectedErrorMessage);
        }

        [Test]
        public void Validate_EmptyCalculatedAt_ShouldBeValidationResultWithExpectedErrorMessage()
        {
            // Arrange
            var systemUnderTest = new DepositCalculationEntity { CalculatedAt = DateTime.MinValue };

            var validationContext = new ValidationContext(systemUnderTest);

            string expectedErrorMessage = DepositCalculationEntityValidationErrors.EmptyCalculatedAt;

            // Act
            IEnumerable<ValidationResult> actualResult = systemUnderTest.Validate(validationContext);

            // Assert
            ContainsErrorMessage(actualResult, expectedErrorMessage);
        }

        [TestCase(0)]
        [TestCase(1000001)]
        public void Validate_DepositAmountNotInclusiveBetween1And1000000_ShouldBeValidationResultWithExpectedErrorMessage(
            decimal depositAmount)
        {
            // Arrange
            var systemUnderTest = new DepositCalculationEntity { DepositAmount = depositAmount };

            var validationContext = new ValidationContext(systemUnderTest);

            string expectedErrorMessage = string.Format(
                DepositCalculationEntityValidationErrors.DepositAmountNotInclusiveBetween1And1000000,
                depositAmount);

            // Act
            IEnumerable<ValidationResult> actualResult = systemUnderTest.Validate(validationContext);

            // Assert
            ContainsErrorMessage(actualResult, expectedErrorMessage);
        }

        [Test]
        public void Validate_NullMonthlyCalculations_ShouldBeValidationResultWithExpectedErrorMessage()
        {
            // Arrange
            var systemUnderTest = new DepositCalculationEntity { MonthlyCalculations = null };

            var validationContext = new ValidationContext(systemUnderTest);

            string expectedErrorMessage = DepositCalculationEntityValidationErrors.NullMonthlyCalculations;

            // Act
            IEnumerable<ValidationResult> actualResult = systemUnderTest.Validate(validationContext);

            // Assert
            ContainsErrorMessage(actualResult, expectedErrorMessage);
        }

        [Test]
        public void Validate_MonthlyCalculationsCountNotEqualPeriodInMonths_ShouldBeValidationResultWithExpectedErrorMessage()
        {
            // Arrange
            var systemUnderTest = new DepositCalculationEntity
            {
                MonthlyCalculations = new MonthlyDepositCalculationEntity[23],
                PeriodInMonths = 32
            };

            var validationContext = new ValidationContext(systemUnderTest);

            string expectedErrorMessage = string.Format(
                DepositCalculationEntityValidationErrors.MonthlyCalculationsCountNotEqualPeriodInMonths,
                systemUnderTest.MonthlyCalculations.Count);

            // Act
            IEnumerable<ValidationResult> actualResult = systemUnderTest.Validate(validationContext);

            // Assert
            ContainsErrorMessage(actualResult, expectedErrorMessage);
        }

        private static IEnumerable<DepositCalculationEntity> ValidDepositCalculationEntityTestCases()
        {
            yield return new DepositCalculationEntity
            {
                DepositAmount = 1000000,
                CalculatedAt = new DateTime(2023, 10, 29),
                Percent = 100,
                PeriodInMonths = 36,
                MonthlyCalculations = new MonthlyDepositCalculationEntity[36]
            };

            yield return new DepositCalculationEntity
            {
                DepositAmount = 192784,
                CalculatedAt = new DateTime(2023, 10, 23),
                Percent = 1,
                PeriodInMonths = 23,
                MonthlyCalculations = new MonthlyDepositCalculationEntity[23]
            };

            yield return new DepositCalculationEntity
            {
                DepositAmount = 1,
                CalculatedAt = new DateTime(2023, 7, 12),
                Percent = 54,
                PeriodInMonths = 11,
                MonthlyCalculations = new MonthlyDepositCalculationEntity[11]
            };
        }

        private void ContainsErrorMessage(IEnumerable<ValidationResult> result, string expectedErrorMessage)
        {
            ValidationResult[] validationResults = result.ToArray();

            result
                .Should()
                .NotBeEmpty()
                .And
                .ContainSingle(validationResult => validationResult.ErrorMessage == expectedErrorMessage);
        }
    }
}