using AutoMapper;
using DepositCalculator.Shared.Models;
using DepositCalculator.UI.Models;

namespace DepositCalculator.UI.Tests.Mapping
{
    [Collection(MapperConfigurationCollection.Name)]
    public class MonthlyDepositCalculationPageModelMappingTests
    {
        private readonly IMapper _mapper;

        public MonthlyDepositCalculationPageModelMappingTests(MapperConfigurationFixture mapperConfigurationFixture)
        {
            _mapper = new Mapper(mapperConfigurationFixture._configuration);
        }

        [Fact]
        internal void Mapping_MonthlyDepositCalculationResponseModel_ShouldReturnMappedMonthlyDepositCalculationPageModel()
        {
            // Arrange
            var fakeMonthlyDepositCalculationResponseModel = new MonthlyDepositCalculationResponseModel(
                monthNumber: 23,
                depositByMonth: 81782.21m,
                totalDepositAmount: 8172.22222m);

            // Act
            MonthlyDepositCalculationPageModel result = _mapper.Map<MonthlyDepositCalculationPageModel>(
                fakeMonthlyDepositCalculationResponseModel);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeMonthlyDepositCalculationResponseModel);
        }
    }
}