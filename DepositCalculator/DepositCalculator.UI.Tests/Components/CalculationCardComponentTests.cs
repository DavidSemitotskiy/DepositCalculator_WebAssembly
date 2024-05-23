using System;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AutoMapper;
using Blazored.Modal;
using Blazored.Modal.Services;
using Bunit;
using DepositCalculator.Shared.Models;
using DepositCalculator.UI.Components;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Models;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace DepositCalculator.UI.Tests.Components
{
    public class CalculationCardComponentTests : TestContext
    {
        private const string ButtonCssSelector = "button";

        private readonly IRenderedComponent<CalculationCardComponent> _systemUnderTest;

        private readonly CalculationDetailsPageModel _fakeCalculationDetailsPageModel;

        private readonly Mock<IModalService> _modalServiceMock;

        private readonly Mock<IMapper> _mapperMock;

        private readonly Mock<ILogger<CalculationCardComponent>> _loggerMock;

        private readonly Mock<IDepositCalculatorApiService> _depositCalculatorApiServiceMock;

        private readonly Mock<IDepositCalculationStateService> _depositCalculationStateServiceMock;

        public CalculationCardComponentTests()
        {
            _fakeCalculationDetailsPageModel = new CalculationDetailsPageModel(
                id: Guid.NewGuid(),
                percent: 33.12m,
                periodInMonths: 33,
                depositAmount: 8172.28m,
                calculatedAt: new DateTime(1972, 12, 10, 4, 26, 51));

            _modalServiceMock = new Mock<IModalService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CalculationCardComponent>>();
            _depositCalculatorApiServiceMock = new Mock<IDepositCalculatorApiService>();
            _depositCalculationStateServiceMock = new Mock<IDepositCalculationStateService>();

            Services.AddScoped(typeof(IModalService), _ => _modalServiceMock.Object);
            Services.AddTransient(typeof(IMapper), _ => _mapperMock.Object);
            Services.AddSingleton(_loggerMock.Object);
            Services.AddTransient(typeof(IDepositCalculatorApiService), _ => _depositCalculatorApiServiceMock.Object);
            Services.AddSingleton(_depositCalculationStateServiceMock.Object);

            _systemUnderTest = RenderComponent<CalculationCardComponent>(parameters => parameters
                .Add(component => component.CalculationDetails, _fakeCalculationDetailsPageModel)
                .Add(component => component.ModalService, _modalServiceMock.Object));
        }

        [Fact]
        internal async Task LoadDepositCalculationAsync_ErrorOccuredWhileLoadingCalculations_ShouldLogError()
        {
            // Arrange
            var fakeErrorMessage = "Error";
            var httpRequestException = new HttpRequestException(fakeErrorMessage);

            _depositCalculatorApiServiceMock
                .Setup(depositCalculatorApiService => depositCalculatorApiService
                    .GetCalculationByIdAsync(_fakeCalculationDetailsPageModel.id))
                .ThrowsAsync(httpRequestException);

            // Act
            await _systemUnderTest.Find(ButtonCssSelector).ClickAsync(new MouseEventArgs());

            // Assert
            _loggerMock.Verify(
                logger => logger.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((message, t) => message.ToString() == httpRequestException.ToString()),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        internal async Task LoadDepositCalculationAsync_LoadingNullDepositCalculationResponseModel_ShouldNotSetCalculationsToContainerAndShowModal()
        {
            // Arrange

            // Act
            await _systemUnderTest.Find(ButtonCssSelector).ClickAsync(new MouseEventArgs());

            // Assert
            _depositCalculationStateServiceMock.Verify(
                depositCalculationStateService => depositCalculationStateService
                    .SetDepositCalculationPageModelAsync(It.IsAny<DepositCalculationPageModel>()),
                Times.Never);

            _modalServiceMock.Verify(
                modalService => modalService.Show<DepositCalculationTableComponent>(),
                Times.Never);
        }

        [Fact]
        internal async Task LoadDepositCalculationAsync_LoadingNotNullDepositCalculationResponseModel_ShouldSetCalculationsToContainerAndShowModal()
        {
            // Arrange
            var fakeMonthlyDepositCalculationResponseModels = new MonthlyDepositCalculationResponseModel[]
            {
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 2,
                    depositByMonth: 412,
                    totalDepositAmount: 1820),
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 34,
                    depositByMonth: 10284,
                    totalDepositAmount: 189)
            };

            var fakeDepositCalculationResponseModel = new DepositCalculationResponseModel(fakeMonthlyDepositCalculationResponseModels);

            _depositCalculatorApiServiceMock
                .Setup(depositCalculatorApiService => depositCalculatorApiService
                    .GetCalculationByIdAsync(_fakeCalculationDetailsPageModel.id))
                .ReturnsAsync(fakeDepositCalculationResponseModel);

            var fakeMonthlyDepositCalculationPageModels = new MonthlyDepositCalculationPageModel[]
            {
                new MonthlyDepositCalculationPageModel(
                    monthNumber: 2,
                    depositByMonth: 412,
                    totalDepositAmount: 1820),
                new MonthlyDepositCalculationPageModel(
                    monthNumber: 34,
                    depositByMonth: 10284,
                    totalDepositAmount: 189)
            };

            var fakeDepositCalculationPageModel = new DepositCalculationPageModel(fakeMonthlyDepositCalculationPageModels);

            _mapperMock
                .Setup(mapper => mapper.Map<DepositCalculationPageModel>(fakeDepositCalculationResponseModel))
                .Returns(fakeDepositCalculationPageModel);

            // Act
            await _systemUnderTest.Find(ButtonCssSelector).ClickAsync(new MouseEventArgs());

            // Assert
            _depositCalculationStateServiceMock.Verify(
                depositCalculationStateService => depositCalculationStateService
                    .SetDepositCalculationPageModelAsync(It.IsAny<DepositCalculationPageModel>()),
                Times.Once);

            _modalServiceMock.Verify(
                modalService => modalService.Show<HistoryDetailsModalComponent>(It.IsAny<string>(), It.IsAny<ModalOptions>()),
                Times.Once);
        }

        [Fact]
        internal void CalculationCardComponent_Rendering_ShouldContainExpectedMarkup()
        {
            // Arrange
            var expectedMarkup =
                @$"<div class=""card-body"">
                      <div class=""property-info"">
                          <label>
                              Percent:
                          </label>
                          <span>33%</span>
                      </div>
                      <div class=""property-info"">
                          <label>
                              Term:
                          </label>
                          <span>33</span>
                      </div>
                      <div class=""property-info"">
                          <label>
                              Sum:
                          </label>
                          <span>8172$</span>
                      </div>
                      <div class=""property-info"">
                          <label>
                              Date:
                          </label>
                          <span>10.12.1972</span>
                      </div>
                      <button class=""custom-btn"">See details</button>
                </div>";

            var cardBodyCssSelector = ".card-body";

            // Act
            IElement actualMarkup = _systemUnderTest.Find(cardBodyCssSelector);

            // Assert
            actualMarkup.MarkupMatches(expectedMarkup);
        }
    }
}