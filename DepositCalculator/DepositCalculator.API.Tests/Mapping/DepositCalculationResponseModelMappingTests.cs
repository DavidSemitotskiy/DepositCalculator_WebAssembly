using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.API.Tests.Mapping
{
    internal class DepositCalculationResponseModelMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_DepositCalculationResponseDTO_ShouldReturnMappedDepositCalculationResponseModel()
        {
            // Arrange
            var fakeMonthlyDepositCalculationResponseDTOs = new MonthlyDepositCalculationResponseDTO[]
            {
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 1,
                    depositByMonth: 9231,
                    totalDepositAmount: 22131),
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 23,
                    depositByMonth: 8126,
                    totalDepositAmount: 6712344)
            };

            var fakeDepositCalculationResponseDTO = new DepositCalculationResponseDTO(fakeMonthlyDepositCalculationResponseDTOs);

            // Act
            DepositCalculationResponseModel result = _mapper.Map<DepositCalculationResponseModel>(fakeDepositCalculationResponseDTO);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeDepositCalculationResponseDTO);
        }
    }
}