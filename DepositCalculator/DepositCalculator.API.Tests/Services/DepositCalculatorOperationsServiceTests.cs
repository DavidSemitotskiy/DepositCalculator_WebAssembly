using System;
using System.Threading.Tasks;
using AutoMapper;
using DepositCalculator.API.Services;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.Shared.Models;
using Moq;

namespace DepositCalculator.API.Tests.Services
{
    internal class DepositCalculatorOperationsServiceTests
    {
        private DepositCalculatorOperationsService _systemUnderTest;

        private DepositCalculationResponseDTO _fakeDepositCalculationResponseDTO;

        private DepositCalculationResponseModel _fakeDepositCalculationResponseModel;

        private Mock<IDepositCalculatorService> _depositCalculatorServiceMock;

        private Mock<IMapper> _mapperMock;

        private DepositInfoRequestModel _dummyDepositInfoRequestModel;

        [SetUp]
        public void Setup()
        {
            _dummyDepositInfoRequestModel = new DepositInfoRequestModel();

            var fakeMonthlyDepositCalculationResponseDTOs = new MonthlyDepositCalculationResponseDTO[]
            {
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 1,
                    depositByMonth: 10,
                    totalDepositAmount: 1212),
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 2,
                    depositByMonth: 123,
                    totalDepositAmount: 1312)
            };

            _fakeDepositCalculationResponseDTO = new DepositCalculationResponseDTO(fakeMonthlyDepositCalculationResponseDTOs);

            var fakeMonthlyDepositCalculationResponseModels = new MonthlyDepositCalculationResponseModel[]
            {
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 1,
                    depositByMonth: 10,
                    totalDepositAmount: 1212),
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 2,
                    depositByMonth: 123,
                    totalDepositAmount: 1312)
            };

            _fakeDepositCalculationResponseModel = new DepositCalculationResponseModel(fakeMonthlyDepositCalculationResponseModels);

            _depositCalculatorServiceMock = new Mock<IDepositCalculatorService>();
            _mapperMock = new Mock<IMapper>();

            _systemUnderTest = new DepositCalculatorOperationsService(_depositCalculatorServiceMock.Object, _mapperMock.Object);
        }

        [Test]
        public async Task CalculateAsync_DepositInfoRequestModel_ShouldReturnMappedDepositCalculationToResponseModel()
        {
            // Arrange
            SetupMapper(_fakeDepositCalculationResponseDTO, _fakeDepositCalculationResponseModel);

            _depositCalculatorServiceMock
                .Setup(calculatorService => calculatorService.CalculateAsync(It.IsAny<DepositInfoRequestDTO>()))
                .ReturnsAsync(_fakeDepositCalculationResponseDTO);

            // Act
            DepositCalculationResponseModel result = await _systemUnderTest.CalculateAsync(_dummyDepositInfoRequestModel);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(_fakeDepositCalculationResponseDTO);
        }

        [Test]
        public async Task GetPaginatedCalculationsAsync_GettingPaginatedCalculationDetails_ShouldReturnMappedPaginatedCalculationDetailsToResponseModel()
        {
            // Arrange
            var dummyPaginationRequest = new PaginationRequestModel();

            var totalCount = 87;

            var date = new DateTime(2341, 3, 24);

            var fakeCalculationDetailsResponseDTOs = new CalculationDetailsResponseDTO[]
            {
                new CalculationDetailsResponseDTO(
                    id: Guid.Empty,
                    percent: 81,
                    periodInMonths: 23,
                    depositAmount: 812,
                    calculatedAt: date)
            };

            var fakePaginatedCalculationResponseDTOs = new PaginatedResponseModel<CalculationDetailsResponseDTO>(
                totalCount,
                fakeCalculationDetailsResponseDTOs);

            _depositCalculatorServiceMock
                .Setup(depositCalculatorService => depositCalculatorService.GetPaginatedCalculationsAsync(It.IsAny<PaginationRequestModel>()))
                .ReturnsAsync(fakePaginatedCalculationResponseDTOs);

            var fakeCalculationDetailsResponseModels = new CalculationDetailsResponseModel[]
            {
                new CalculationDetailsResponseModel(
                    id: Guid.Empty,
                    percent: 81,
                    periodInMonths: 23,
                    depositAmount: 812,
                    calculatedAt: date)
            };

            var fakePaginatedCalculationResponseModels = new PaginatedResponseModel<CalculationDetailsResponseModel>(
                totalCount,
                fakeCalculationDetailsResponseModels);

            SetupMapper(fakePaginatedCalculationResponseDTOs, fakePaginatedCalculationResponseModels);

            // Act
            PaginatedResponseModel<CalculationDetailsResponseModel> result = await _systemUnderTest
                .GetPaginatedCalculationsAsync(dummyPaginationRequest);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakePaginatedCalculationResponseDTOs);
        }

        [Test]
        public async Task GetCalculationByIdAsync_DepositCalculationId_ShouldReturnMappedDepositCalculationToResponseModel()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            _depositCalculatorServiceMock
                .Setup(depositCalculatorService => depositCalculatorService.GetCalculationByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(_fakeDepositCalculationResponseDTO);

            SetupMapper(_fakeDepositCalculationResponseDTO, _fakeDepositCalculationResponseModel);

            // Act
            DepositCalculationResponseModel? result = await _systemUnderTest.GetCalculationByIdAsync(id);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(_fakeDepositCalculationResponseDTO);
        }

        private void SetupMapper<TSource, TDestination>(TSource source, TDestination destination)
        {
            _mapperMock
                .Setup(mapper => mapper.Map<TDestination>(source))
                .Returns(destination);
        }
    }
}