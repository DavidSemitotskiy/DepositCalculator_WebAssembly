using DepositCalculator.BLL.DTOs;
using DepositCalculator.Entities;

namespace DepositCalculator.BLL.Tests.Mapping
{
    internal class MonthlyDepositCalculationEntityMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_MonthlyDepositCalculationResponseDTO_ShouldReturnMappedMonthlyDepositCalculationEntity()
        {
            // Arrange
            var fakeMonthlyDepositCalculationResponseDTO = new MonthlyDepositCalculationResponseDTO(
                monthNumber: 81,
                depositByMonth: 9172,
                totalDepositAmount: 81672);

            // Act
            MonthlyDepositCalculationEntity result = _mapper.Map<MonthlyDepositCalculationEntity>(fakeMonthlyDepositCalculationResponseDTO);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeMonthlyDepositCalculationResponseDTO, options => options
                    .WithMapping(
                        nameof(MonthlyDepositCalculationResponseDTO.monthNumber),
                        nameof(MonthlyDepositCalculationEntity.MonthNumber))
                    .WithMapping(
                        nameof(MonthlyDepositCalculationResponseDTO.depositByMonth),
                        nameof(MonthlyDepositCalculationEntity.DepositByMonth))
                    .WithMapping(
                        nameof(MonthlyDepositCalculationResponseDTO.totalDepositAmount),
                        nameof(MonthlyDepositCalculationEntity.TotalDepositAmount)));
        }
    }
}