using System;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.Entities;

namespace DepositCalculator.BLL.Tests.Mapping
{
    internal class CalculationDetailsResponseDTOMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_DepositCalculationEntity_ShouldReturnMappedCalculationDetailsResponseDTO()
        {
            // Arrange
            var fakeDepositCalculationEntity = new DepositCalculationEntity
            {
                Id = Guid.NewGuid(),
                Percent = 72,
                PeriodInMonths = 23,
                DepositAmount = 81672,
                CalculatedAt = new DateTime(2341),
                MonthlyCalculations = new MonthlyDepositCalculationEntity[0]
            };

            // Act
            CalculationDetailsResponseDTO result = _mapper.Map<CalculationDetailsResponseDTO>(fakeDepositCalculationEntity);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeDepositCalculationEntity, options => options.ExcludingMissingMembers());
        }
    }
}