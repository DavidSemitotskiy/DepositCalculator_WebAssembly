using System;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.Entities;

namespace DepositCalculator.BLL.Tests.Mapping
{
    internal class DepositCalculationResponseDTOMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_DepositCalculationEntity_ShouldReturnMappedDepositCalculationResponseDTO()
        {
            // Arrange
            var fakeDepositCalculationEntity = new DepositCalculationEntity
            {
                MonthlyCalculations = new MonthlyDepositCalculationEntity[]
                {
                    new MonthlyDepositCalculationEntity
                    {
                        Id = Guid.NewGuid(),
                        MonthNumber = 4,
                        DepositByMonth = 82,
                        DepositCalculationId = Guid.NewGuid(),
                        TotalDepositAmount = 9182
                    },
                    new MonthlyDepositCalculationEntity
                    {
                        Id = Guid.NewGuid(),
                        MonthNumber = 2,
                        DepositByMonth = 1351,
                        DepositCalculationId = Guid.NewGuid(),
                        TotalDepositAmount = 7612
                    }
                }
            };

            // Act
            var result = _mapper.Map<DepositCalculationResponseDTO>(fakeDepositCalculationEntity);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeDepositCalculationEntity, options => options.ExcludingMissingMembers());
        }
    }
}