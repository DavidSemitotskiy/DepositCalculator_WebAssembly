using System;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.API.Tests.Mapping
{
    internal class PaginatedResponseModelMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_PaginatedCalculationDetailsResponseDTO_ShouldReturnMappedPaginatedCalculationDetailsResponseModel()
        {
            // Arrange
            var totalCount = 36;

            var fakeCalculationDetailsResponseDTOs = new CalculationDetailsResponseDTO[]
            {
                new CalculationDetailsResponseDTO(
                    id: Guid.NewGuid(),
                    percent: 81,
                    periodInMonths: 23,
                    depositAmount: 812,
                    calculatedAt: new DateTime(1893, 11, 6))
            };

            var fakePaginatedCalculationResponseDTOs = new PaginatedResponseModel<CalculationDetailsResponseDTO>(
                totalCount,
                fakeCalculationDetailsResponseDTOs);

            // Act
            PaginatedResponseModel<CalculationDetailsResponseModel> result = _mapper
                .Map<PaginatedResponseModel<CalculationDetailsResponseModel>>(fakePaginatedCalculationResponseDTOs);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakePaginatedCalculationResponseDTOs);
        }
    }
}