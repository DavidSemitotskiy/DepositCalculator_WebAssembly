using System;
using DepositCalculator.BLL.Exceptions;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.Shared.Enums;
using Moq;

namespace DepositCalculator.BLL.Tests.Services
{
    internal class DepositCalculationStrategyResolverTests
    {
        private DepositCalculationStrategyResolver _systemUnderTest;

        private Mock<IDepositCalculationStrategyFactory> _strategyFactoryMock;

        [SetUp]
        public void Setup()
        {
            var simpleDepositCalculationStrategyMock = new Mock<IDepositCalculationStrategy>();

            simpleDepositCalculationStrategyMock
                .Setup(simpleDepositCalculationStrategy => simpleDepositCalculationStrategy.CalculationMethod)
                .Returns(DepositCalculationMethod.Simple);

            var compoundDepositCalculationStrategyMock = new Mock<IDepositCalculationStrategy>();

            compoundDepositCalculationStrategyMock
                .Setup(compoundDepositCalculationStrategy => compoundDepositCalculationStrategy.CalculationMethod)
                .Returns(DepositCalculationMethod.Compound);

            _strategyFactoryMock = new Mock<IDepositCalculationStrategyFactory>();

            _strategyFactoryMock
                .Setup(strategyFactory => strategyFactory.Create(DepositCalculationMethod.Simple))
                .Returns(simpleDepositCalculationStrategyMock.Object);

            _strategyFactoryMock
                .Setup(strategyFactory => strategyFactory.Create(DepositCalculationMethod.Compound))
                .Returns(compoundDepositCalculationStrategyMock.Object);

            _systemUnderTest = new DepositCalculationStrategyResolver(_strategyFactoryMock.Object);
        }

        [TestCase(-10)]
        [TestCase(102)]
        [TestCase(3)]
        public void GetStrategy_InvalidCalculationMethod_ShouldThrowStrategyNotFoundException(
            DepositCalculationMethod invalidCalculationMethod)
        {
            // Arrange
            var expectedErrorMessage = $"There is not such {invalidCalculationMethod} strategy";

            // Act
            Action actualException = () => _systemUnderTest.GetStrategy(invalidCalculationMethod);

            // Assert
            actualException
                .Should()
                .Throw<StrategyNotFoundException>()
                .WithMessage(expectedErrorMessage);
        }

        [Theory]
        public void GetStrategy_ValidCalculationMethod_ShouldReturnDepositCalculationStrategy(
            DepositCalculationMethod validCalculationMethod)
        {
            // Arrange

            // Act
            IDepositCalculationStrategy result = _systemUnderTest.GetStrategy(validCalculationMethod);

            // Assert
            result
                .Should()
                .NotBeNull();

            result.CalculationMethod
                .Should()
                .Be(validCalculationMethod);
        }
    }
}