using System;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using Blazored.FluentValidation;
using Bunit;
using DepositCalculator.Shared.Models;
using DepositCalculator.UI.Components;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace DepositCalculator.UI.Tests.Components
{
    public class DepositFormComponentTests : TestContext
    {
        private const string ButtonCssSelector = "button";

        private readonly IRenderedComponent<DepositFormComponent> _systemUnderTest;

        private readonly Mock<IMapper> _mapperMock;

        private readonly Mock<IDepositCalculatorApiService> _depositCalculatorApiServiceMock;

        private readonly Mock<IDepositCalculationStateService> _depositCalculationStateServiceMock;

        private readonly Mock<ILogger<DepositFormComponent>> _loggerMock;

        public DepositFormComponentTests()
        {
            _mapperMock = new Mock<IMapper>();
            _depositCalculatorApiServiceMock = new Mock<IDepositCalculatorApiService>();
            _depositCalculationStateServiceMock = new Mock<IDepositCalculationStateService>();
            _loggerMock = new Mock<ILogger<DepositFormComponent>>();

            Services.AddTransient(typeof(IMapper), _ => _mapperMock.Object);
            Services.AddTransient(typeof(IDepositCalculatorApiService), _ => _depositCalculatorApiServiceMock.Object);
            Services.AddSingleton(_depositCalculationStateServiceMock.Object);
            Services.AddSingleton(_loggerMock.Object);

            ComponentFactories.AddStub<FluentValidationValidator>();
            ComponentFactories.AddStub<ValidationSummary>();

            _systemUnderTest = RenderComponent<DepositFormComponent>();
        }

        [Fact]
        internal async Task SubmitDepositInfoAsync_GettingNullDepositCalculationResponseModel_ShouldNotSetCalculationsToContainer()
        {
            // Arrange

            // Act
            await _systemUnderTest.Find(ButtonCssSelector).ClickAsync(new MouseEventArgs());

            // Assert
            _depositCalculationStateServiceMock.Verify(
                depositCalculationStateService => depositCalculationStateService
                    .SetDepositCalculationPageModelAsync(It.IsAny<DepositCalculationPageModel>()),
                Times.Never);
        }

        [Fact]
        internal async Task SubmitDepositInfoAsync_GettingNotNullDepositCalculationResponseModel_ShouldSetCalculationsToContainer()
        {
            // Arrange
            var fakeDepositCalculationResponseModels = new DepositCalculationResponseModel(new MonthlyDepositCalculationResponseModel[1]);

            _depositCalculatorApiServiceMock
                .Setup(depositCalculatorApiService => depositCalculatorApiService.CalculateAsync(It.IsAny<DepositInfoRequestModel>()))
                .ReturnsAsync(fakeDepositCalculationResponseModels);

            // Act
            await _systemUnderTest.Find(ButtonCssSelector).ClickAsync(new MouseEventArgs());

            // Assert
            _depositCalculationStateServiceMock.Verify(
                depositCalculationStateService => depositCalculationStateService
                    .SetDepositCalculationPageModelAsync(It.IsAny<DepositCalculationPageModel>()),
                Times.Once);
        }

        [Fact]
        internal async Task SubmitDepositInfoAsync_ErrorOccuredWhileGettingCalculations_ShouldLogError()
        {
            // Arrange
            var fakeErrorMessage = "Error";
            var httpRequestException = new HttpRequestException(fakeErrorMessage);

            _depositCalculatorApiServiceMock
                .Setup(depositCalculatorApiService => depositCalculatorApiService.CalculateAsync(It.IsAny<DepositInfoRequestModel>()))
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
    }
}