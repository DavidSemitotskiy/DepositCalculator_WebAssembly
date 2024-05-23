using DepositCalculator.DAL.Interfaces;
using DepositCalculator.DAL.Services;
using FluentAssertions;
using Moq;

namespace DepositCalculator.DAL.Tests.Services
{
    internal class DepositCalculatorContextTests
    {
        private DepositCalculatorContext _systemUnderTest;

        private Mock<IDbContextOptionsWrapper<DepositCalculatorContext>> _optionsMock;

        private Mock<IValidatorWrapper> _validatorMock;

        [SetUp]
        public void Setup()
        {
            _optionsMock = new Mock<IDbContextOptionsWrapper<DepositCalculatorContext>>();
            _validatorMock = new Mock<IValidatorWrapper>();

            _systemUnderTest = new DepositCalculatorContext(_optionsMock.Object, _validatorMock.Object);
        }

        [Test]
        public void DbSets_InitializingDbContext_DbSetsShouldBeNotNull()
        {
            // Arrange

            // Act

            // Assert
            _systemUnderTest.DepositCalculations
                .Should()
                .NotBeNull();
        }
    }
}