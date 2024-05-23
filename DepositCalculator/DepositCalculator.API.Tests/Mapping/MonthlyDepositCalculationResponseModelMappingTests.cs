using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.API.Tests.Mapping
{
    internal class MonthlyDepositCalculationResponseModelMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_MonthlyDepositCalculationResponseDTO_ShouldReturnMappedMonthlyDepositCalculationResponseModel()
        {
            // Arrange
            var fakeMonthlyDepositCalculationResponseDTO = new MonthlyDepositCalculationResponseDTO(
                monthNumber: 8,
                depositByMonth: 212,
                totalDepositAmount: 3412);

            // Act
            MonthlyDepositCalculationResponseModel result = _mapper.Map<MonthlyDepositCalculationResponseModel>(
                fakeMonthlyDepositCalculationResponseDTO);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeMonthlyDepositCalculationResponseDTO);
        }
    }
}