using System;
using DepositCalculator.BLL.Exceptions;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.Shared.Enums;
using Moq;

namespace DepositCalculator.BLL.Tests.Services
{
    internal class DepositCalculationStrategyFactoryTests
    {
        private IList<IDepositCalculationStrategy> _fakeRegisteredCalculationStrategies;

        private DepositCalculationStrategyFactory _systemUnderTest;

        [SetUp]
        public void Setup()
        {
            _fakeRegisteredCalculationStrategies = new List<IDepositCalculationStrategy>();

            _systemUnderTest = new DepositCalculationStrategyFactory(_fakeRegisteredCalculationStrategies);
        }

        [Theory]
        public void Create_ValidCalculationMethod_ShouldReturnDepositCalculationStrategy(
            DepositCalculationMethod validCalculationMethod)
        {
            // Arrange
            var depositCalculationStrategyMock = new Mock<IDepositCalculationStrategy>();

            depositCalculationStrategyMock
                .Setup(depositCalculationStrategyMock => depositCalculationStrategyMock.CalculationMethod)
                .Returns(validCalculationMethod);

            _fakeRegisteredCalculationStrategies.Add(depositCalculationStrategyMock.Object);

            // Act
            IDepositCalculationStrategy result = _systemUnderTest.Create(validCalculationMethod);

            // Assert
            result
                .Should()
                .NotBeNull();

            result.CalculationMethod
                .Should()
                .Be(validCalculationMethod);
        }

        [TestCase(-10)]
        [TestCase(3)]
        public void Create_InvalidCalculationMethod_ShouldThrowStrategyNotRegisterredException(
            DepositCalculationMethod invalidCalculationMethod)
        {
            // Arrange
            var expectedErrorMessage = $"There is not such {invalidCalculationMethod} strategy registerred to the container";

            // Act
            Action actualException = () => _systemUnderTest.Create(invalidCalculationMethod);

            // Assert
            actualException
                .Should()
                .Throw<StrategyNotRegisterredException>()
                .WithMessage(expectedErrorMessage);
        }
    }
}