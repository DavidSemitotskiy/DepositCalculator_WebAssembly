using System;
using AutoMapper;

namespace DepositCalculator.UI.Tests.Mapping
{
    [Collection(MapperConfigurationCollection.Name)]
    public class MappingConfigurationTests
    {
        private readonly IMapper _mapper;

        public MappingConfigurationTests(MapperConfigurationFixture mapperConfigurationFixture)
        {
            _mapper = new Mapper(mapperConfigurationFixture._configuration);
        }

        [Fact]
        internal void Mapper_MapperConfiguration_ShouldBeValid()
        {
            // Arrange

            // Act
            Action result = () => _mapper.ConfigurationProvider.AssertConfigurationIsValid();

            // Assert
            result
                .Should()
                .NotThrow<AutoMapperConfigurationException>();
        }
    }
}