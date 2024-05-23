using System;
using AutoMapper;
using DepositCalculator.Shared.Models;
using DepositCalculator.UI.Models;

namespace DepositCalculator.UI.Tests.Mapping
{
    [Collection(MapperConfigurationCollection.Name)]
    public class CalculationDetailsPageModelMappingTests
    {
        private readonly IMapper _mapper;

        public CalculationDetailsPageModelMappingTests(MapperConfigurationFixture mapperConfigurationFixture)
        {
            _mapper = new Mapper(mapperConfigurationFixture._configuration);
        }

        [Fact]
        internal void Mapping_CalculationDetailsResponseModel_ShouldReturnMappedCalculationDetailsPageModel()
        {
            // Arrange
            var fakeCalculationDetailsResponseModel = new CalculationDetailsResponseModel(
                id: Guid.NewGuid(),
                percent: 22,
                periodInMonths: 23,
                depositAmount: 9182,
                calculatedAt: new DateTime(2007, 10, 14));

            // Act
            CalculationDetailsPageModel result = _mapper.Map<CalculationDetailsPageModel>(fakeCalculationDetailsResponseModel);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeCalculationDetailsResponseModel);
        }
    }
}