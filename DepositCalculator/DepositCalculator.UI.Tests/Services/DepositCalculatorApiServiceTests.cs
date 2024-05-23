using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Utils;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Services;
using DepositCalculator.UI.Utils;
using Microsoft.Extensions.Options;
using Moq;

namespace DepositCalculator.UI.Tests.Services
{
    public class DepositCalculatorApiServiceTests
    {
        private readonly DepositCalculatorApiService _systemUnderTests;

        private readonly DepositCalculationResponseModel _fakeDepositCalculationResponseModel;

        private readonly PaginationRequestModel _fakePaginationRequest;

        private readonly Mock<IDepositCalculatorHttpClientWrapper> _depositCalculatorHttpClientWrapperMock;

        private readonly Mock<IOptions<DepositCalculatorApiOptions>> _depositCalculatorApiOptionsMock;

        private readonly HttpResponseMessage _fakeHttpResponseMessage;

        public DepositCalculatorApiServiceTests()
        {
            _fakePaginationRequest = new PaginationRequestModel();

            var fakeMonthlyDepositCalculationResponseModels = new MonthlyDepositCalculationResponseModel[]
            {
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 6,
                    depositByMonth: 71827m,
                    totalDepositAmount: 9128.72671m),
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 2,
                    depositByMonth: 2512m,
                    totalDepositAmount: 79981m)
            };

            _fakeDepositCalculationResponseModel = new DepositCalculationResponseModel(fakeMonthlyDepositCalculationResponseModels);

            _fakeHttpResponseMessage = new HttpResponseMessage();

            _depositCalculatorHttpClientWrapperMock = new Mock<IDepositCalculatorHttpClientWrapper>();

            _depositCalculatorHttpClientWrapperMock
                .Setup(depositCalculatorHttpService => depositCalculatorHttpService
                    .PostAsync(It.IsAny<string>(), It.IsAny<DepositInfoRequestModel>()))
                .ReturnsAsync(_fakeHttpResponseMessage);

            _depositCalculatorHttpClientWrapperMock
                .Setup(depositCalculatorHttpService => depositCalculatorHttpService.GetAsync(It.IsAny<string>()))
                .ReturnsAsync(_fakeHttpResponseMessage);

            var fakeDepositCalculatorApiOptions = new DepositCalculatorApiOptions
            {
                CalculateEndpoint = "/calculateendpoint",
                CalculationsEndpoint = "/calculationsendpoint"
            };

            _depositCalculatorApiOptionsMock = new Mock<IOptions<DepositCalculatorApiOptions>>();

            _depositCalculatorApiOptionsMock
                .Setup(depositCalculatorApiOptions => depositCalculatorApiOptions.Value)
                .Returns(fakeDepositCalculatorApiOptions);

            _systemUnderTests = new DepositCalculatorApiService(
                _depositCalculatorHttpClientWrapperMock.Object,
                _depositCalculatorApiOptionsMock.Object);
        }

        [Fact]
        internal async Task CalculateAsync_SuccessResponseStatusCode_ShouldReturnDepositCalculationResponseModel()
        {
            // Arrange
            var dummyDepositInfoRequestModel = new DepositInfoRequestModel();

            _fakeHttpResponseMessage.StatusCode = HttpStatusCode.OK;
            _fakeHttpResponseMessage.Content = JsonContent.Create(_fakeDepositCalculationResponseModel);

            // Act
            DepositCalculationResponseModel? result = await _systemUnderTests.CalculateAsync(dummyDepositInfoRequestModel);

            // Assert
            result
                .Should()
                .NotBeNull();

            result
                .Should()
                .BeEquivalentTo(_fakeDepositCalculationResponseModel);
        }

        [Fact]
        internal async Task CalculateAsync_NotSuccessResponseStatusCode_ShouldReturnNullDepositCalculationResponseModel()
        {
            // Arrange
            var dummyDepositInfoRequestModel = new DepositInfoRequestModel();

            _fakeHttpResponseMessage.StatusCode = HttpStatusCode.NotFound;

            // Act
            DepositCalculationResponseModel? result = await _systemUnderTests.CalculateAsync(dummyDepositInfoRequestModel);

            // Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        internal async Task GetPaginatedCalculationsAsync_SuccessResponseStatusCode_ShouldReturnPaginatedCalculationsDetailsResponseModel()
        {
            // Arrange
            var totalCount = 45;

            _fakeHttpResponseMessage.Headers.Add(ResponseHeaderConstants.TotalCount, totalCount.ToString());

            var fakeCalculationDetailsResponseModels = new CalculationDetailsResponseModel[]
            {
                new CalculationDetailsResponseModel(
                    id: Guid.NewGuid(),
                    percent: 92,
                    periodInMonths: 13,
                    depositAmount: 812,
                    calculatedAt: new DateTime(2008, 10, 11))
            };

            _fakeHttpResponseMessage.StatusCode = HttpStatusCode.OK;
            _fakeHttpResponseMessage.Content = JsonContent.Create(fakeCalculationDetailsResponseModels);

            // Act
            PaginatedResponseModel<CalculationDetailsResponseModel>? result = await _systemUnderTests
                .GetPaginatedCalculationsAsync(_fakePaginationRequest);

            // Assert
            result
                .Should()
                .NotBeNull();

            result.items
                .Should()
                .BeEquivalentTo(fakeCalculationDetailsResponseModels);

            result.totalCount
                .Should()
                .Be(totalCount);
        }

        [Fact]
        internal async Task GetPaginatedCalculationsAsync_NotSuccessResponseStatusCode_ShouldReturnNullPaginatedCalculationsDetailsResponseModel()
        {
            // Arrange
            _fakeHttpResponseMessage.StatusCode = HttpStatusCode.BadRequest;

            // Act
            PaginatedResponseModel<CalculationDetailsResponseModel>? result = await _systemUnderTests
                .GetPaginatedCalculationsAsync(_fakePaginationRequest);

            // Assert
            result
                .Should()
                .BeNull();
        }

        [Fact]
        internal async Task GetCalculationByIdAsync_SuccessResponseStatusCode_ShouldReturnDepositCalculationResponseModel()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _fakeHttpResponseMessage.StatusCode = HttpStatusCode.OK;
            _fakeHttpResponseMessage.Content = JsonContent.Create(_fakeDepositCalculationResponseModel);

            // Act
            DepositCalculationResponseModel? result = await _systemUnderTests.GetCalculationByIdAsync(id);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(_fakeDepositCalculationResponseModel);
        }

        [Fact]
        internal async Task GetCalculationByIdAsync_NotSuccessResponseStatusCode_ShouldReturnNullDepositCalculationResponseModel()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _fakeHttpResponseMessage.StatusCode = HttpStatusCode.InternalServerError;

            // Act
            DepositCalculationResponseModel? result = await _systemUnderTests.GetCalculationByIdAsync(id);

            // Assert
            result
                .Should()
                .BeNull();
        }
    }
}