using DepositCalculator.BLL.DTOs;
using DepositCalculator.Entities;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Tests.Mapping
{
    internal class DepositCalculationEntityMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_DepositCalculationResponseDTO_ShouldReturnMappedDepositCalculationEntity()
        {
            // Arrange
            var fakeMonthlyDepositCalculationResponseDTOs = new MonthlyDepositCalculationResponseDTO[]
            {
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 2,
                    depositByMonth: 8162,
                    totalDepositAmount: 98173),
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 71,
                    depositByMonth: 7451,
                    totalDepositAmount: 42614)
            };

            var fakeDepositCalculationResponseDTO = new DepositCalculationResponseDTO(fakeMonthlyDepositCalculationResponseDTOs);

            // Act
            DepositCalculationEntity result = _mapper.Map<DepositCalculationEntity>(fakeDepositCalculationResponseDTO);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeDepositCalculationResponseDTO, options => options
                    .WithMapping(
                        nameof(DepositCalculationResponseDTO.monthlyCalculations),
                        nameof(DepositCalculationEntity.MonthlyCalculations))
                    .WithMapping<MonthlyDepositCalculationResponseDTO, MonthlyDepositCalculationEntity>(
                        monthlyDTO => monthlyDTO.monthNumber,
                        monthlyEntity => monthlyEntity.MonthNumber)
                    .WithMapping<MonthlyDepositCalculationResponseDTO, MonthlyDepositCalculationEntity>(
                        monthlyDTO => monthlyDTO.depositByMonth,
                        monthlyEntity => monthlyEntity.DepositByMonth)
                    .WithMapping<MonthlyDepositCalculationResponseDTO, MonthlyDepositCalculationEntity>(
                        monthlyDTO => monthlyDTO.totalDepositAmount,
                        monthlyEntity => monthlyEntity.TotalDepositAmount));
        }

        [Test]
        public void Mapping_DepositInfoRequestDTO_ShouldReturnMappedDepositCalculationEntity()
        {
            // Arrange
            var depositInfoRequestDTO = new DepositInfoRequestDTO(
                depositAmount: 8172,
                periodInMonths: 12,
                percent: 15,
                calculationMethod: DepositCalculationMethod.Simple);

            // Act
            DepositCalculationEntity result = _mapper.Map<DepositCalculationEntity>(depositInfoRequestDTO);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(depositInfoRequestDTO, options => options.ExcludingMissingMembers());
        }
    }
}