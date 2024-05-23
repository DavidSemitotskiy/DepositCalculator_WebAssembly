using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Dom;
using AutoMapper;
using Bunit;
using Castle.Core.Internal;
using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Utils;
using DepositCalculator.UI.Components;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Models;
using DepositCalculator.UI.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace DepositCalculator.UI.Tests.Pages
{
    public class HistoryTests : TestContext
    {
        private const string CardCssSelector = ".card";

        private const string ButtonCssSelector = "button";

        private readonly Mock<IDepositCalculatorApiService> _depositCalculatorApiServiceMock;

        private readonly Mock<IMapper> _mapperMock;

        private readonly Mock<ILogger<History>> _loggerMock;

        private readonly CalculationDetailsPageModel[] _fakeCalculationDetails;

        public HistoryTests()
        {
            _fakeCalculationDetails = new CalculationDetailsPageModel[PaginationConstants.MaxPageSize];

            _depositCalculatorApiServiceMock = new Mock<IDepositCalculatorApiService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<History>>();

            Services.AddTransient(typeof(IDepositCalculatorApiService), _ => _depositCalculatorApiServiceMock.Object);
            Services.AddTransient(typeof(IMapper), _ => _mapperMock.Object);
            Services.AddSingleton(_loggerMock.Object);

            var replacementMarkup = @"<div class=""card""><div>";

            ComponentFactories.AddStub<CalculationCardComponent>(replacementMarkup);
        }

        [Fact]
        internal void History_RouteAttribute_ShouldBeValidRoute()
        {
            // Arrange
            var expectedRoute = "/history";
            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            // Act
            RouteAttribute actualRouteAttribute = systemUnderTest.Instance.GetType().GetAttribute<RouteAttribute>();

            // Assert
            actualRouteAttribute
                .Should()
                .NotBeNull();

            actualRouteAttribute.Template
                .Should()
                .Be(expectedRoute);
        }

        [Fact]
        internal void OnInitializedAsync_GettingNullPaginatedCalculationsByFirstPage_ShouldNotContainMarkup()
        {
            // Arrange

            // Act
            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            // Assert
            systemUnderTest.Markup
                .Should()
                .BeEmpty();
        }

        [Fact]
        internal void OnInitializedAsync_GettingNotNullPaginatedCalculationsByFirstPage_ShouldHaveMarkup()
        {
            // Arrange
            PaginatedResponseModel<CalculationDetailsResponseModel> fakePaginatedCalculationDetails
                = GeneratePaginatedCalculations();

            SetupGetCalculationsByPageAsyncToReturn(fakePaginatedCalculationDetails);

            SetupMapper(
                fakePaginatedCalculationDetails.items,
                (IReadOnlyCollection<CalculationDetailsPageModel>)_fakeCalculationDetails);

            // Act
            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            // Assert
            WaitLoadingPaginatedCalculations(systemUnderTest);

            systemUnderTest.Markup
                .Should()
                .NotBeEmpty();
        }

        [Fact]
        internal void OnInitializedAsync_GettingNotNullPaginatedCalculationsByFirstPage_ShouldShowExpectedCalculations()
        {
            // Arrange
            PaginatedResponseModel<CalculationDetailsResponseModel> fakePaginatedCalculationDetails
                = GeneratePaginatedCalculations();

            SetupGetCalculationsByPageAsyncToReturn(fakePaginatedCalculationDetails);

            SetupMapper(
                fakePaginatedCalculationDetails.items,
                (IReadOnlyCollection<CalculationDetailsPageModel>)_fakeCalculationDetails);

            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            WaitLoadingPaginatedCalculations(systemUnderTest);

            // Act
            IRefreshableElementCollection<IElement> calculationCardComponents = systemUnderTest.FindAll(CardCssSelector);

            // Assert
            calculationCardComponents.Count
                .Should()
                .Be(_fakeCalculationDetails.Length);
        }

        [Fact]
        internal async Task LoadPaginatedCalculationsAsync_GettingNotNullPaginatedCalculationsByNextPage_ShouldShowExpectedCalculations()
        {
            // Arrange
            var expectedCalculationsCount = 32;
            var totalCount = 45;

            PaginatedResponseModel<CalculationDetailsResponseModel> fakePaginatedCalculationDetails
                = GeneratePaginatedCalculations(totalCount);

            SetupGetCalculationsByPageAsyncToReturn(fakePaginatedCalculationDetails);

            SetupMapper(
                fakePaginatedCalculationDetails.items,
                (IReadOnlyCollection<CalculationDetailsPageModel>)_fakeCalculationDetails);

            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            WaitLoadingPaginatedCalculations(systemUnderTest);

            await systemUnderTest.Find(ButtonCssSelector).ClickAsync(new MouseEventArgs());

            // Act
            systemUnderTest.Render();

            // Assert
            IRefreshableElementCollection<IElement> calculationCardComponents = systemUnderTest.FindAll(CardCssSelector);

            calculationCardComponents.Count
                .Should()
                .Be(expectedCalculationsCount);
        }

        [Fact]
        internal async Task LoadPaginatedCalculationsAsync_GettingNullPaginatedCalculationsByNextPage_ShouldNotChangeExistedCalculations()
        {
            // Arrange
            int expectedCalculationsCount = _fakeCalculationDetails.Length;

            var totalCount = 20;

            PaginatedResponseModel<CalculationDetailsResponseModel> fakePaginatedCalculationDetails
                = GeneratePaginatedCalculations(totalCount);

            SetupGetCalculationsByPageAsyncToReturn(fakePaginatedCalculationDetails);

            SetupMapper(
                fakePaginatedCalculationDetails.items,
                (IReadOnlyCollection<CalculationDetailsPageModel>)_fakeCalculationDetails);

            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            WaitLoadingPaginatedCalculations(systemUnderTest);

            var nextPage = (PaginatedResponseModel<CalculationDetailsResponseModel>)null;

            SetupGetCalculationsByPageAsyncToReturn(nextPage);

            await systemUnderTest.Find(ButtonCssSelector).ClickAsync(new MouseEventArgs());

            // Act
            systemUnderTest.Render();

            // Assert
            IRefreshableElementCollection<IElement> calculationCardComponents = systemUnderTest.FindAll(CardCssSelector);

            calculationCardComponents.Count
                .Should()
                .Be(expectedCalculationsCount);
        }

        [Fact]
        internal void LoadPaginatedCalculationsAsync_HasNextPage_ShouldIncrementPageNumber()
        {
            // Arrange
            var totalCount = 54;

            PaginatedResponseModel<CalculationDetailsResponseModel> fakePaginatedCalculationDetails
                = GeneratePaginatedCalculations(totalCount);

            var expectedPageNumber = 2;

            SetupGetCalculationsByPageAsyncToReturn(fakePaginatedCalculationDetails);

            SetupMapper(
                fakePaginatedCalculationDetails.items,
                (IReadOnlyCollection<CalculationDetailsPageModel>)_fakeCalculationDetails);

            // Act
            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            // Assert
            WaitLoadingPaginatedCalculations(systemUnderTest);

            systemUnderTest.Instance.PaginationRequest.PageNumber
                .Should()
                .Be(expectedPageNumber);
        }

        [Fact]
        internal void LoadPaginatedCalculationsAsync_HasNotNextPage_ShouldNotIncrementPageNumber()
        {
            // Arrange
            PaginatedResponseModel<CalculationDetailsResponseModel> fakePaginatedCalculationDetails
                = GeneratePaginatedCalculations();

            var expectedPageNumber = 1;

            SetupGetCalculationsByPageAsyncToReturn(fakePaginatedCalculationDetails);

            SetupMapper(
                fakePaginatedCalculationDetails.items,
                (IReadOnlyCollection<CalculationDetailsPageModel>)_fakeCalculationDetails);

            // Act
            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            // Assert
            WaitLoadingPaginatedCalculations(systemUnderTest);

            systemUnderTest.Instance.PaginationRequest.PageNumber
                .Should()
                .Be(expectedPageNumber);
        }

        [Fact]
        internal void LoadPaginatedCalculationsAsync_HasNextPage_ShouldNotDisableLoadMoreButton()
        {
            // Arrange
            var totalCount = 98;

            PaginatedResponseModel<CalculationDetailsResponseModel> fakePaginatedCalculationDetails
                = GeneratePaginatedCalculations(totalCount);

            SetupGetCalculationsByPageAsyncToReturn(fakePaginatedCalculationDetails);

            SetupMapper(
                fakePaginatedCalculationDetails.items,
                (IReadOnlyCollection<CalculationDetailsPageModel>)_fakeCalculationDetails);

            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            WaitLoadingPaginatedCalculations(systemUnderTest);

            // Act
            IElement result = systemUnderTest.Find(ButtonCssSelector);

            // Assert
            result
                .IsDisabled()
                .Should()
                .BeFalse();
        }

        [Fact]
        internal void LoadPaginatedCalculationsAsync_HasNotNextPage_ShouldDisableLoadMoreButton()
        {
            // Arrange
            PaginatedResponseModel<CalculationDetailsResponseModel> fakePaginatedCalculationDetails
                = GeneratePaginatedCalculations();

            SetupGetCalculationsByPageAsyncToReturn(fakePaginatedCalculationDetails);

            SetupMapper(
                fakePaginatedCalculationDetails.items,
                (IReadOnlyCollection<CalculationDetailsPageModel>)_fakeCalculationDetails);

            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

            WaitLoadingPaginatedCalculations(systemUnderTest);

            // Act
            IElement result = systemUnderTest.Find(ButtonCssSelector);

            // Assert
            result
                .IsDisabled()
                .Should()
                .BeTrue();
        }

        [Fact]
        internal void LoadPaginatedCalculationsAsync_ErrorOccuredWhileLoadingPaginatedCalculations_ShouldLogError()
        {
            // Arrange
            var fakeErrorMessage = "Error";
            var httpRequestException = new HttpRequestException(fakeErrorMessage);

            _depositCalculatorApiServiceMock
                .Setup(depositCalculatorApiService => depositCalculatorApiService
                    .GetPaginatedCalculationsAsync(It.IsAny<PaginationRequestModel>()))
                .ThrowsAsync(httpRequestException);

            // Act
            IRenderedComponent<History> systemUnderTest = RenderComponent<History>();

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

        private PaginatedResponseModel<CalculationDetailsResponseModel> GeneratePaginatedCalculations(
            int totalCount = default)
        {
            var fakeCalculationDetails = new CalculationDetailsResponseModel[0];

            return new PaginatedResponseModel<CalculationDetailsResponseModel>(
                totalCount,
                fakeCalculationDetails);
        }

        private void SetupGetCalculationsByPageAsyncToReturn(PaginatedResponseModel<CalculationDetailsResponseModel> paginatedCalculationDetails)
        {
            _depositCalculatorApiServiceMock
                .Setup(depositCalculatorApiService => depositCalculatorApiService
                    .GetPaginatedCalculationsAsync(It.IsAny<PaginationRequestModel>()))
                .ReturnsAsync(paginatedCalculationDetails);
        }

        private void SetupMapper<TSource, TDestination>(TSource source, TDestination destination)
        {
            _mapperMock
                .Setup(mapper => mapper.Map<TDestination>(source))
                .Returns(destination);
        }

        private void WaitLoadingPaginatedCalculations(IRenderedComponent<History> component)
        {
            component.WaitForState(() => component.Instance.CalculationsHistory
                .DepositCalculations.Count != 0);
        }
    }
}