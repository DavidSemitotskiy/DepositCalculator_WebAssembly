using System;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.Entities;

namespace DepositCalculator.BLL.Tests.Mapping
{
    internal class MonthlyDepositCalculationResponseDTOMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_MonthlyDepositCalculationEntity_ShouldReturnMappedMonthlyDepositCalculationResponseDTO()
        {
            // Arrange
            var fakeMonthlyDepositCalculationEntity = new MonthlyDepositCalculationEntity
            {
                Id = Guid.NewGuid(),
                MonthNumber = 81,
                DepositByMonth = 91283,
                TotalDepositAmount = 812,
                DepositCalculationId = Guid.NewGuid()
            };

            // Act
            MonthlyDepositCalculationResponseDTO result = _mapper.Map<MonthlyDepositCalculationResponseDTO>(
                fakeMonthlyDepositCalculationEntity);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeMonthlyDepositCalculationEntity, options => options.ExcludingMissingMembers());
        }
    }
}