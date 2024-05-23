using System;
using DepositCalculator.API.Filters;
using DepositCalculator.BLL.Exceptions;
using DepositCalculator.Shared.Errors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DepositCalculator.API.Tests.Filters
{
    internal class CalculationStrategyExceptionFilterAttributeTests : BaseFilterAttributeTests
    {
        private CalculationStrategyExceptionFilterAttribute _systemUnderTest;

        private ExceptionContext _fakeExceptionContext;

        [SetUp]
        public void Setup()
        {
            _systemUnderTest = new CalculationStrategyExceptionFilterAttribute();

            _fakeExceptionContext = new ExceptionContext(_fakeActionContext, new List<IFilterMetadata>());
        }

        [TestCaseSource(nameof(StrategyExceptionsTestCases))]
        public void OnException_StrategyExceptionOccurs_ShouldSetExceptionContextResultWithJsonResult(StrategyException fakeStrategyException)
        {
            // Arrange
            _fakeExceptionContext.Exception = fakeStrategyException;

            // Act
            _systemUnderTest.OnException(_fakeExceptionContext);

            // Assert
            _fakeExceptionContext.Result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<JsonResult>();

            var jsonResult = _fakeExceptionContext.Result as JsonResult;

            jsonResult.StatusCode
                .Should()
                .Be(fakeStrategyException.StatusCode);

            jsonResult.Value
                .Should()
                .BeOfType<CalculationStrategyError>();

            var calculationStrategyError = jsonResult.Value as CalculationStrategyError;

            calculationStrategyError.ErrorMessage
                .Should()
                .Be(fakeStrategyException.Message);
        }

        [TestCaseSource(nameof(RandomExceptionsTestCases))]
        public void OnException_RandomExceptionOccurs_ExceptionContextResultShouldBeNull(Exception exception)
        {
            // Arrange
            _fakeExceptionContext.Exception = exception;

            // Act
            _systemUnderTest.OnException(_fakeExceptionContext);

            // Assert
            _fakeExceptionContext.Result
                .Should()
                .BeNull();
        }

        private static IEnumerable<StrategyException> StrategyExceptionsTestCases()
        {
            yield return new StrategyNotFoundException("Message");

            yield return new StrategyNotRegisterredException("Message");
        }

        private static IEnumerable<Exception> RandomExceptionsTestCases()
        {
            yield return new Exception();

            yield return new IndexOutOfRangeException();

            yield return new StackOverflowException();
        }
    }
}