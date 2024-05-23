using AutoMapper;
using DepositCalculator.Shared.Models;
using DepositCalculator.UI.Models;

namespace DepositCalculator.UI.Tests.Mapping
{
    [Collection(MapperConfigurationCollection.Name)]
    public class DepositCalculationPageModelMappingTests
    {
        private readonly IMapper _mapper;

        public DepositCalculationPageModelMappingTests(MapperConfigurationFixture mapperConfigurationFixture)
        {
            _mapper = new Mapper(mapperConfigurationFixture._configuration);
        }

        [Fact]
        internal void Mapping_DepositCalculationResponseModel_ShouldReturnMappedDepositCalculationPageModel()
        {
            // Arrange
            var fakeMonthlyDepositCalculations = new MonthlyDepositCalculationResponseModel[]
            {
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 3,
                    depositByMonth: 8172.912m,
                    totalDepositAmount: 9891m),
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 1,
                    depositByMonth: 2517m,
                    totalDepositAmount: 77712m)
            };

            var fakeDepositCalculationResponseModel = new DepositCalculationResponseModel(fakeMonthlyDepositCalculations);

            // Act
            DepositCalculationPageModel result = _mapper.Map<DepositCalculationPageModel>(fakeDepositCalculationResponseModel);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeDepositCalculationResponseModel);
        }
    }
}