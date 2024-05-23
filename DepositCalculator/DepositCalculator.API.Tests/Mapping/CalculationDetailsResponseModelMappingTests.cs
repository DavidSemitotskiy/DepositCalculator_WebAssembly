using System;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.API.Tests.Mapping
{
    internal class CalculationDetailsResponseModelMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_CalculationDetailsResponseDTO_ShouldReturnMappedCalculationDetailsResponseModel()
        {
            // Arrange
            var fakeCalculationDetailsResponseDTO = new CalculationDetailsResponseDTO(
                id: Guid.NewGuid(),
                percent: 23,
                periodInMonths: 23,
                depositAmount: 912,
                calculatedAt: new DateTime(1763, 2, 23));

            // Act
            CalculationDetailsResponseModel result = _mapper.Map<CalculationDetailsResponseModel>(fakeCalculationDetailsResponseDTO);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeCalculationDetailsResponseDTO);
        }
    }
}