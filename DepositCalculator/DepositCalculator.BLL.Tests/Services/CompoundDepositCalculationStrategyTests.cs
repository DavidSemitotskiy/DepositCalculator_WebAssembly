using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Tests.Services
{
    internal class CompoundDepositCalculationStrategyTests
    {
        private CompoundDepositCalculationStrategy _systemUnderTest;

        [SetUp]
        public void Setup()
        {
            _systemUnderTest = new CompoundDepositCalculationStrategy();
        }

        [Test]
        public void CalculationMethod_GettingCalculationMethod_ShouldReturnCompoundDepositCalculationMethod()
        {
            // Arrange
            var expectedDepositCalculationMethod = DepositCalculationMethod.Compound;

            // Act
            DepositCalculationMethod actualDepositCalculationMethod = _systemUnderTest.CalculationMethod;

            // Arrange
            actualDepositCalculationMethod
                .Should()
                .Be(expectedDepositCalculationMethod);
        }

        [Test]
        public void Calculate_DepositInfoRequestDTO_ShouldBeEqualCountMonthlyCalculationsToPeriodInMonths()
        {
            // Arrange
            var fakeDepositInfoRequest = new DepositInfoRequestDTO(
                depositAmount: default,
                periodInMonths: 4,
                percent: default,
                calculationMethod: DepositCalculationMethod.Compound);

            // Act
            DepositCalculationResponseDTO result = _systemUnderTest.Calculate(fakeDepositInfoRequest);

            // Assert
            result.monthlyCalculations
                .Should()
                .HaveCount(fakeDepositInfoRequest.periodInMonths);
        }

        [TestCaseSource(nameof(CalculationResultTestCases))]
        public void Calculate_DepositInfoRequestDTO_ShouldBeCompoundDeposit(
            DepositInfoRequestDTO depositInfoRequestDTO,
            DepositCalculationResponseDTO expectedResult)
        {
            // Arrange

            // Act
            DepositCalculationResponseDTO actualResult = _systemUnderTest.Calculate(depositInfoRequestDTO);

            // Assert
            actualResult
                .Should()
                .BeEquivalentTo(expectedResult);
        }

        private static IEnumerable<object[]> CalculationResultTestCases()
        {
            yield return new object[]
            {
                new DepositInfoRequestDTO(
                    depositAmount: 1000,
                    periodInMonths: 6,
                    percent: 10,
                    calculationMethod: DepositCalculationMethod.Compound),
                new DepositCalculationResponseDTO(new MonthlyDepositCalculationResponseDTO[]
                {
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 1,
                        depositByMonth: 8.3333333333333333333333333M,
                        totalDepositAmount: 1008.3333333333333333333333333M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 2,
                        depositByMonth: 16.7361111111111111111111110M,
                        totalDepositAmount: 1016.7361111111111111111111110M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 3,
                        depositByMonth: 25.2089120370370370370370369M,
                        totalDepositAmount: 1025.2089120370370370370370369M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 4,
                        depositByMonth: 33.7523196373456790123456788M,
                        totalDepositAmount: 1033.7523196373456790123456788M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 5,
                        depositByMonth: 42.3669223009902263374485594M,
                        totalDepositAmount: 1042.3669223009902263374485594M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 6,
                        depositByMonth: 51.0533133201651448902606307M,
                        totalDepositAmount: 1051.0533133201651448902606307M)
                })
            };

            yield return new object[]
            {
                new DepositInfoRequestDTO(
                    depositAmount: 6251,
                    periodInMonths: 3,
                    percent: 15,
                    calculationMethod: DepositCalculationMethod.Compound),
                new DepositCalculationResponseDTO(new MonthlyDepositCalculationResponseDTO[]
                {
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 1,
                        depositByMonth: 78.1375M,
                        totalDepositAmount: 6329.1375M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 2,
                        depositByMonth: 157.25171875M,
                        totalDepositAmount: 6408.25171875M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 3,
                        depositByMonth: 237.354865234375M,
                        totalDepositAmount: 6488.354865234375M)
                })
            };
        }
    }
}