using System;
using System.Threading.Tasks;
using AutoMapper;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.DAL.Interfaces;
using DepositCalculator.Entities;
using DepositCalculator.Shared.Enums;
using DepositCalculator.Shared.Models;
using Moq;

namespace DepositCalculator.BLL.Tests.Services
{
    internal class DepositCalculatorServiceTests
    {
        private DepositCalculatorService _systemUnderTest;

        private Mock<IMapper> _mapperMock;

        private Mock<IDepositCalculationStrategy> _calculationStrategyMock;

        private Mock<IDepositCalculationStrategyResolver> _strategyResolverMock;

        private Mock<IDepositCalculationRepository> _depositCalculationRepositoryMock;

        private Mock<IDateTimeWrapper> _timeMock;

        private DepositInfoRequestDTO _fakeDepositInfoRequestDTO;

        private DepositCalculationEntity _fakeDepositCalculationEntity;

        [SetUp]
        public void Setup()
        {
            var fakeMonthlyCalculationDTOs = new MonthlyDepositCalculationResponseDTO[]
            {
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 1,
                    depositByMonth: 2121,
                    totalDepositAmount: 102102),
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 21,
                    depositByMonth: 812,
                    totalDepositAmount: 91782)
            };

            var fakeDepositCalculationResponseDTO = new DepositCalculationResponseDTO(fakeMonthlyCalculationDTOs);

            _fakeDepositCalculationEntity = new DepositCalculationEntity();

            _fakeDepositInfoRequestDTO = new DepositInfoRequestDTO(
                depositAmount: default,
                periodInMonths: default,
                percent: default,
                calculationMethod: default);

            _mapperMock = new Mock<IMapper>();
            _calculationStrategyMock = new Mock<IDepositCalculationStrategy>();
            _strategyResolverMock = new Mock<IDepositCalculationStrategyResolver>();
            _depositCalculationRepositoryMock = new Mock<IDepositCalculationRepository>();
            _timeMock = new Mock<IDateTimeWrapper>();

            _mapperMock
                .Setup(mapper => mapper.Map(It.IsAny<DepositInfoRequestDTO>(), It.IsAny<DepositCalculationEntity>()))
                .Returns(_fakeDepositCalculationEntity);

            _strategyResolverMock
                .Setup(strategyResolver => strategyResolver.GetStrategy(It.IsAny<DepositCalculationMethod>()))
                .Returns(_calculationStrategyMock.Object);

            _calculationStrategyMock
                .Setup(calculationStrategy => calculationStrategy.Calculate(It.IsAny<DepositInfoRequestDTO>()))
                .Returns(fakeDepositCalculationResponseDTO);

            _systemUnderTest = new DepositCalculatorService(
                _mapperMock.Object,
                _strategyResolverMock.Object,
                _depositCalculationRepositoryMock.Object,
                _timeMock.Object);
        }

        [Test]
        public async Task CalculateAsync_DepositInfoRequestDTO_ShouldReturnMappedDepositCalculationEntityToDTOAndStoreCalculatedDepositByStrategy()
        {
            // Arrange

            // Act
            DepositCalculationResponseDTO result = await _systemUnderTest.CalculateAsync(_fakeDepositInfoRequestDTO);

            // Assert
            _depositCalculationRepositoryMock.Verify(
                depositCalculationRepository => depositCalculationRepository.AddAsync(_fakeDepositCalculationEntity),
                Times.Once);

            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<DepositCalculationResponseDTO>();
        }

        [Test]
        public async Task CalculateAsync_ValidDepositCalculation_ShouldSetExpectedTimeForCalculatedAtProperty()
        {
            // Arrange
            var fakeNowTime = new DateTime(2023, 11, 6, 11, 3, 23);

            _timeMock
                .Setup(timeService => timeService.Now)
                .Returns(fakeNowTime);

            // Act
            DepositCalculationResponseDTO result = await _systemUnderTest.CalculateAsync(_fakeDepositInfoRequestDTO);

            // Assert
            _depositCalculationRepositoryMock.Verify(
                depositCalculationRepository => depositCalculationRepository
                    .AddAsync(It.Is<DepositCalculationEntity>(entity => entity.CalculatedAt == fakeNowTime)),
                Times.Once);
        }

        [Test]
        public async Task GetCalculationByIdAsync_DepositCalculationId_ShouldReturnMappedDepositCalculationResponseDTO()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            var fakeMonthlyDepositCalculationEntities = new MonthlyDepositCalculationEntity[]
            {
                new MonthlyDepositCalculationEntity
                {
                    Id = Guid.NewGuid(),
                    MonthNumber = 91,
                    DepositByMonth = 812,
                    TotalDepositAmount = 91828,
                    DepositCalculationId = Guid.NewGuid()
                },
                new MonthlyDepositCalculationEntity
                {
                    Id = Guid.NewGuid(),
                    MonthNumber = 4,
                    DepositByMonth = 918282,
                    TotalDepositAmount = 31241,
                    DepositCalculationId = Guid.NewGuid()
                }
            };

            var fakeDepositCalculationEntity = new DepositCalculationEntity
            {
                Id = id,
                MonthlyCalculations = fakeMonthlyDepositCalculationEntities
            };

            _depositCalculationRepositoryMock
                .Setup(depositCalculationRepository => depositCalculationRepository.GetByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(fakeDepositCalculationEntity);

            var fakeMonthlyDepositCalculationDTOs = new MonthlyDepositCalculationResponseDTO[]
            {
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 91,
                    depositByMonth: 812,
                    totalDepositAmount: 91828),
                new MonthlyDepositCalculationResponseDTO(
                    monthNumber: 4,
                    depositByMonth: 918282,
                    totalDepositAmount: 31241)
            };

            var fakeDepositCalculationResponseDTO = new DepositCalculationResponseDTO(fakeMonthlyDepositCalculationDTOs);

            SetupMapper(fakeDepositCalculationEntity, fakeDepositCalculationResponseDTO);

            // Act
            DepositCalculationResponseDTO? result = await _systemUnderTest.GetCalculationByIdAsync(id);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(fakeDepositCalculationEntity, options => options.ExcludingMissingMembers());
        }

        [Test]
        public async Task GetPaginatedCalculationsAsync_GettingCalculationsByPageFromDatabase_ShouldReturnPaginatedCalculationDetailsResponseDTO()
        {
            // Arrange
            var dummyPaginationRequest = new PaginationRequestModel();

            var expectedTotalCount = 10;

            var fakeDepositCalculationEntities = new DepositCalculationEntity[]
            {
                new DepositCalculationEntity
                {
                    Id = Guid.Empty,
                    DepositAmount = 901,
                    Percent = 10,
                    PeriodInMonths = 23,
                    MonthlyCalculations = new MonthlyDepositCalculationEntity[0],
                    CalculatedAt = new DateTime(1874, 2, 23)
                }
            };

            _depositCalculationRepositoryMock
                .Setup(depositCalculationRepository => depositCalculationRepository.GetByPageAsync(It.IsAny<PaginationRequestModel>()))
                .ReturnsAsync(fakeDepositCalculationEntities);

            _depositCalculationRepositoryMock
                .Setup(depositCalculationRepository => depositCalculationRepository.CountAsync())
                .ReturnsAsync(expectedTotalCount);

            var fakeCalculationDetailsResponseDTOs = new CalculationDetailsResponseDTO[]
            {
                new CalculationDetailsResponseDTO(
                    id: Guid.Empty,
                    percent: 10,
                    periodInMonths: 23,
                    depositAmount: 901,
                    calculatedAt: new DateTime(1874, 2, 23))
            };

            SetupMapper(
                fakeDepositCalculationEntities,
                (IReadOnlyCollection<CalculationDetailsResponseDTO>)fakeCalculationDetailsResponseDTOs);

            // Act
            PaginatedResponseModel<CalculationDetailsResponseDTO> actualResult = await _systemUnderTest
                .GetPaginatedCalculationsAsync(dummyPaginationRequest);

            // Assert
            actualResult
                .Should()
                .NotBeNull();

            actualResult.items
                .Should()
                .BeEquivalentTo(fakeDepositCalculationEntities, options => options.ExcludingMissingMembers());

            actualResult.totalCount
                .Should()
                .Be(expectedTotalCount);
        }

        private void SetupMapper<TSource, TDestination>(TSource source, TDestination destination)
        {
            _mapperMock
                .Setup(mapper => mapper.Map<TDestination>(source))
                .Returns(destination);
        }
    }
}