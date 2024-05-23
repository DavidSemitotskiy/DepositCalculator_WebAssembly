using System;
using AutoMapper;

namespace DepositCalculator.BLL.Tests.Mapping
{
    internal class MappingConfigurationTests : BaseMappingTests
    {
        [Test]
        public void Mapper_MapperConfiguration_ShouldBeValid()
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