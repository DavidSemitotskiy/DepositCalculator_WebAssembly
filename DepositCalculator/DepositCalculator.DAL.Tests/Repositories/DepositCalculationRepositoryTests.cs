using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using DepositCalculator.DAL.Interfaces;
using DepositCalculator.DAL.Repositories;
using DepositCalculator.DAL.Services;
using DepositCalculator.Entities;
using DepositCalculator.Shared.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Moq;

namespace DepositCalculator.DAL.Tests.Repositories
{
    internal class DepositCalculationRepositoryTests
    {
        private DepositCalculationRepository _systemUnderTest;

        private Mock<IAsyncQueryProvider> _asyncQueryProviderMock;

        private Mock<DbSet<DepositCalculationEntity>> _databaseSetMock;

        private Mock<IDbContextWrapper<DepositCalculatorContext>> _databaseContextMock;

        private IQueryable<DepositCalculationEntity> _fakeDepositCalculations;

        [SetUp]
        public void Setup()
        {
            _fakeDepositCalculations = new DepositCalculationEntity[]
            {
                new DepositCalculationEntity
                {
                    Id = Guid.NewGuid(),
                    DepositAmount = 28163,
                    Percent = 54,
                    PeriodInMonths = 2,
                    CalculatedAt = new DateTime(1984, 11, 4)
                },
                new DepositCalculationEntity
                {
                    Id = Guid.NewGuid(),
                    DepositAmount = 8172,
                    Percent = 23,
                    PeriodInMonths = 36,
                    CalculatedAt = new DateTime(2871, 9, 23)
                },
                new DepositCalculationEntity
                {
                    Id = Guid.NewGuid(),
                    DepositAmount = 98163,
                    Percent = 4,
                    PeriodInMonths = 21,
                    CalculatedAt = new DateTime(2004, 3, 11)
                },
                new DepositCalculationEntity
                {
                    Id = Guid.NewGuid(),
                    DepositAmount = 16743,
                    Percent = 45,
                    PeriodInMonths = 19,
                    CalculatedAt = new DateTime(2014, 7, 5)
                },
                new DepositCalculationEntity
                {
                    Id = Guid.NewGuid(),
                    DepositAmount = 23871,
                    Percent = 98,
                    PeriodInMonths = 32,
                    CalculatedAt = new DateTime(1783, 1, 29)
                }
            }.AsQueryable();

            _databaseSetMock = new Mock<DbSet<DepositCalculationEntity>>();

            IAsyncEnumerable<DepositCalculationEntity> asyncEnumerable = AsAsyncEnumerable(_fakeDepositCalculations);

            var queryableMock = new Mock<IQueryable<DepositCalculationEntity>>();

            queryableMock
                .Setup(queryable => queryable.Expression)
                .Returns(_fakeDepositCalculations.Expression);

            queryableMock.As<IAsyncEnumerable<DepositCalculationEntity>>()
                .Setup(queryable => queryable.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(asyncEnumerable.GetAsyncEnumerator());

            _asyncQueryProviderMock = new Mock<IAsyncQueryProvider>();

            queryableMock.As<IOrderedQueryable<DepositCalculationEntity>>()
                .Setup(queryable => queryable.Provider)
                .Returns(_asyncQueryProviderMock.Object);

            _asyncQueryProviderMock
                .Setup(asyncQueryProvider => asyncQueryProvider.CreateQuery<DepositCalculationEntity>(It.IsAny<Expression>()))
                .Returns(queryableMock.Object);

            _databaseSetMock.As<IQueryable<DepositCalculationEntity>>()
               .Setup(databaseSet => databaseSet.Provider)
               .Returns(_asyncQueryProviderMock.Object);

            _databaseSetMock.As<IQueryable<DepositCalculationEntity>>()
                .Setup(databaseSet => databaseSet.Expression)
                .Returns(_fakeDepositCalculations.Expression);

            _databaseContextMock = new Mock<IDbContextWrapper<DepositCalculatorContext>>();

            _databaseContextMock
                .Setup(depositCalculatorContext => depositCalculatorContext.Set<DepositCalculationEntity>())
                .Returns(_databaseSetMock.Object);

            _systemUnderTest = new DepositCalculationRepository(_databaseContextMock.Object);
        }

        [Test]
        public async Task AddAsync_AddingDepositCalculation_ShouldAddDepositCalculationToContextAndSaveChanges()
        {
            // Arrange
            var fakeDepositCalculation = new DepositCalculationEntity
            {
                DepositAmount = 8713,
                PeriodInMonths = 2,
                Percent = 78,
                CalculatedAt = new DateTime(2003, 2, 27)
            };

            // Act
            await _systemUnderTest.AddAsync(fakeDepositCalculation);

            // Assert
            _databaseSetMock.Verify(
                depositCalculationDbSet => depositCalculationDbSet
                    .AddAsync(fakeDepositCalculation, It.IsAny<CancellationToken>()),
                Times.Once);

            _databaseContextMock.Verify(
                depositCalculatorContext => depositCalculatorContext.SaveChangesAsync(It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Test]
        public async Task GetByPageAsync_PaginationRequest_ShouldReturnPaginatedDepositCalculations()
        {
            // Arrange
            var paginationRequest = new PaginationRequestModel
            {
                PageNumber = 1,
                PageSize = 4
            };

            // Act
            IEnumerable<DepositCalculationEntity> result = await _systemUnderTest.GetByPageAsync(paginationRequest);

            // Assert
            result
                .Should()
                .NotBeNull();

            result
                .Should()
                .BeSubsetOf(_fakeDepositCalculations);
        }

        [Test]
        public async Task CountAsync_GettingTotalCount_ShouldReturnTotalCount()
        {
            // Arrange

            // Act
            await _systemUnderTest.CountAsync();

            // Assert
            VerifyExecutingAsync<Task<int>>(QueryableMethods.CountWithoutPredicate, Times.Once);
        }

        [Test]
        public async Task GetByIdAsync_DepositCalculationId_ShouldReturnFirstOrDefaultDepositCalculation()
        {
            // Arrange
            Guid id = Guid.NewGuid();

            // Act
            await _systemUnderTest.GetByIdAsync(id);

            // Assert
            VerifyExecutingAsync<Task<DepositCalculationEntity?>>(QueryableMethods.FirstOrDefaultWithPredicate, Times.Once);
        }

        private static async IAsyncEnumerable<DepositCalculationEntity> AsAsyncEnumerable(
            IQueryable<DepositCalculationEntity> calculations)
        {
            foreach (DepositCalculationEntity calculation in calculations)
            {
                yield return calculation;
            }

            await Task.CompletedTask;
        }

        private void VerifyExecutingAsync<TExpectedResult>(MethodInfo methodInfo, Func<Times> times)
        {
            _asyncQueryProviderMock.Verify(
                asyncQueryProvider => asyncQueryProvider.ExecuteAsync<TExpectedResult>(
                    It.Is<Expression>(expression => ((MethodCallExpression)expression).Method.GetType() == methodInfo.GetType()),
                    It.IsAny<CancellationToken>()),
                times);
        }
    }
}