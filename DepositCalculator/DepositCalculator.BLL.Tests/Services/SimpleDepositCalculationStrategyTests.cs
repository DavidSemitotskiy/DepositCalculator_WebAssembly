using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Tests.Services
{
    internal class SimpleDepositCalculationStrategyTests
    {
        private SimpleDepositCalculationStrategy _systemUnderTest;

        [SetUp]
        public void Setup()
        {
            _systemUnderTest = new SimpleDepositCalculationStrategy();
        }

        [Test]
        public void CalculationMethod_GettingCalculationMethod_ShouldReturnSimpleDepositCalculationMethod()
        {
            // Arrange
            var expectedDepositCalculationMethod = DepositCalculationMethod.Simple;

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
                periodInMonths: 8,
                percent: default,
                calculationMethod: DepositCalculationMethod.Simple);

            // Act
            DepositCalculationResponseDTO result = _systemUnderTest.Calculate(fakeDepositInfoRequest);

            // Assert
            result.monthlyCalculations
                .Should()
                .HaveCount(fakeDepositInfoRequest.periodInMonths);
        }

        [TestCaseSource(nameof(CalculationResultTestCases))]
        public void Calculate_DepositInfoRequestDTO_ShouldBeSimpleDeposit(
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
                    depositAmount: 8127,
                    periodInMonths: 7,
                    percent: 18,
                    calculationMethod: DepositCalculationMethod.Simple),
                new DepositCalculationResponseDTO(new MonthlyDepositCalculationResponseDTO[]
                {
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 1,
                        depositByMonth: 121.90499999999999999999999995M,
                        totalDepositAmount: 8248.905000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 2,
                        depositByMonth: 243.80999999999999999999999990M,
                        totalDepositAmount: 8370.810000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 3,
                        depositByMonth: 365.71499999999999999999999985M,
                        totalDepositAmount: 8492.715000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 4,
                        depositByMonth: 487.61999999999999999999999980M,
                        totalDepositAmount: 8614.620000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 5,
                        depositByMonth: 609.52499999999999999999999975M,
                        totalDepositAmount: 8736.525000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 6,
                        depositByMonth: 731.42999999999999999999999970M,
                        totalDepositAmount: 8858.430000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 7,
                        depositByMonth: 853.3349999999999999999999996M,
                        totalDepositAmount: 8980.335000000000000000000000M)
                })
            };

            yield return new object[]
            {
                new DepositInfoRequestDTO(
                    depositAmount: 1923,
                    periodInMonths: 3,
                    percent: 34,
                    calculationMethod: DepositCalculationMethod.Simple),
                new DepositCalculationResponseDTO(new MonthlyDepositCalculationResponseDTO[]
                {
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 1,
                        depositByMonth: 54.484999999999999999999999978M,
                        totalDepositAmount: 1977.4850000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 2,
                        depositByMonth: 108.96999999999999999999999996M,
                        totalDepositAmount: 2031.9700000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 3,
                        depositByMonth: 163.45499999999999999999999994M,
                        totalDepositAmount: 2086.4549999999999999999999999M)
                })
            };

            yield return new object[]
            {
                new DepositInfoRequestDTO(
                    depositAmount: 3742,
                    periodInMonths: 4,
                    percent: 12,
                    calculationMethod: DepositCalculationMethod.Simple),
                new DepositCalculationResponseDTO(new MonthlyDepositCalculationResponseDTO[]
                {
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 1,
                        depositByMonth: 37.419999999999999999999999985M,
                        totalDepositAmount: 3779.4200000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 2,
                        depositByMonth: 74.839999999999999999999999970M,
                        totalDepositAmount: 3816.8400000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 3,
                        depositByMonth: 112.25999999999999999999999996M,
                        totalDepositAmount: 3854.2600000000000000000000000M),
                    new MonthlyDepositCalculationResponseDTO(
                        monthNumber: 4,
                        depositByMonth: 149.67999999999999999999999994M,
                        totalDepositAmount: 3891.6799999999999999999999999M)
                })
            };
        }
    }
}