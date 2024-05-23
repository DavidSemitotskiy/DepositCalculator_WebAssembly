using System;
using System.Threading.Tasks;
using DepositCalculator.UI.Models;
using DepositCalculator.UI.Services;
using Moq;

namespace DepositCalculator.UI.Tests.Services
{
    public class DepositCalculationStateServiceTests
    {
        private readonly DepositCalculationStateService _systemUnderTest;

        private readonly Mock<Func<Task>> _onChangeAsyncMock;

        private readonly DepositCalculationPageModel _fakeDepositCalculationPageModel;

        public DepositCalculationStateServiceTests()
        {
            var fakeMonthlyDepositCalculations = new MonthlyDepositCalculationPageModel[]
            {
                new MonthlyDepositCalculationPageModel(
                    monthNumber: 2,
                    depositByMonth: 192,
                    totalDepositAmount: 2),
                new MonthlyDepositCalculationPageModel(
                    monthNumber: 10,
                    depositByMonth: 917.212m,
                    totalDepositAmount: 82712.2m)
            };

            _fakeDepositCalculationPageModel = new DepositCalculationPageModel(fakeMonthlyDepositCalculations);

            _onChangeAsyncMock = new Mock<Func<Task>>();

            _systemUnderTest = new DepositCalculationStateService();
        }

        [Fact]
        internal async Task SetDepositCalculationPageModelAsync_NotHaveSubscribers_ShouldSetDepositCalculationPageModelPropertyWithoutNotifyingSubscribers()
        {
            // Arrange

            // Act
            await _systemUnderTest.SetDepositCalculationPageModelAsync(_fakeDepositCalculationPageModel);

            // Assert
            _systemUnderTest.CalculationPageModel
                .Should()
                .BeEquivalentTo(_fakeDepositCalculationPageModel);

            _onChangeAsyncMock.Verify(onChangeAsync => onChangeAsync.Invoke(), Times.Never);
        }

        [Fact]
        internal async Task SetDepositCalculationPageModelAsync_HaveSubscribers_ShouldSetDepositCalculationPageModelPropertyAndNotifyAllSubscribers()
        {
            // Arrange
            _systemUnderTest.OnChangeAsync += _onChangeAsyncMock.Object;

            // Act
            await _systemUnderTest.SetDepositCalculationPageModelAsync(_fakeDepositCalculationPageModel);

            // Assert
            _systemUnderTest.CalculationPageModel
                .Should()
                .BeEquivalentTo(_fakeDepositCalculationPageModel);

            _onChangeAsyncMock.Verify(onChangeAsync => onChangeAsync.Invoke(), Times.Once);
        }

        [Fact]
        internal async Task ClearDepositCalculationPageModel_NotEmptyMonthlyCalculations_ShouldCreateNewDepositCalculationPageModel()
        {
            // Arrange
            await _systemUnderTest.SetDepositCalculationPageModelAsync(_fakeDepositCalculationPageModel);

            // Act
            _systemUnderTest.ClearDepositCalculationPageModel();

            // Assert
            _systemUnderTest.CalculationPageModel.monthlyCalculations
                .Should()
                .BeEmpty();
        }

        [Fact]
        internal void ClearDepositCalculationPageModel_EmptyMonthlyCalculations_ShouldNotCreateNewDepositCalculationPageModel()
        {
            // Arrange
            DepositCalculationPageModel depositCalculationPageModel = _systemUnderTest.CalculationPageModel;

            // Act
            _systemUnderTest.ClearDepositCalculationPageModel();

            // Assert
            _systemUnderTest.CalculationPageModel
                .Should()
                .Be(depositCalculationPageModel);
        }
    }
}