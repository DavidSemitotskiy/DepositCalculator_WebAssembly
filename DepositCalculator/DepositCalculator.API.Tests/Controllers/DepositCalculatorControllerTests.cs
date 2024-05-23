using System;
using System.Net.Mime;
using System.Reflection;
using System.Threading.Tasks;
using Castle.Core.Internal;
using DepositCalculator.API.Controllers;
using DepositCalculator.API.Filters;
using DepositCalculator.API.Interfaces;
using DepositCalculator.API.Utils;
using DepositCalculator.Shared.Models;
using DepositCalculator.Shared.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Moq;

namespace DepositCalculator.API.Tests.Controllers
{
    internal class DepositCalculatorControllerTests
    {
        private DepositCalculatorController _systemUnderTest;

        private Mock<IDepositCalculatorOperationsService> _depositCalculatorOperationsServiceMock;

        private DepositCalculationResponseModel _fakeDepositCalculationResponseModel;

        private PaginationRequestModel _dummyPaginationRequestModel;

        [SetUp]
        public void Setup()
        {
            var fakeMonthlyDepositCalculationResponseModels = new MonthlyDepositCalculationResponseModel[]
            {
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 3,
                    depositByMonth: 3141,
                    totalDepositAmount: 8172),
                new MonthlyDepositCalculationResponseModel(
                    monthNumber: 2,
                    depositByMonth: 812,
                    totalDepositAmount: 91782)
            };

            _dummyPaginationRequestModel = new PaginationRequestModel();

            _fakeDepositCalculationResponseModel = new DepositCalculationResponseModel(fakeMonthlyDepositCalculationResponseModels);

            _depositCalculatorOperationsServiceMock = new Mock<IDepositCalculatorOperationsService>();

            _systemUnderTest = new DepositCalculatorController(_depositCalculatorOperationsServiceMock.Object);

            _systemUnderTest.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        #region Attributes Verification

        [Test]
        public void DepositCalculatorController_RouteAttribute_ShouldBeValidRoute()
        {
            // Arrange
            string expectedRouteTemplate = DepositCalculatorControllerRoutes.ControllerRoute;

            // Act
            RouteAttribute actualRouteAttribute = _systemUnderTest.GetType().GetAttribute<RouteAttribute>();

            // Assert
            actualRouteAttribute.Template
                .Should()
                .Be(expectedRouteTemplate);
        }

        [Test]
        public void GetPaginatedCalculationsAsync_HttpGetAttribute_ShouldBeAppliedWithExpectedRouteTemplate()
        {
            // Arrange
            MethodInfo? methodInfo = _systemUnderTest.GetType().GetMethod(nameof(_systemUnderTest.GetPaginatedCalculationsAsync));
            string expectedRouteTemplate = DepositCalculatorControllerRoutes.CalculationsRoute;

            // Act
            HttpMethodAttribute actualResult = methodInfo.GetAttribute<HttpMethodAttribute>();

            // Assert
            VerifyHttpMethod<HttpGetAttribute>(actualResult, expectedRouteTemplate);
        }

        [Test]
        public void GetCalculationByIdAsync_HttpGetAttribute_ShouldBeAppliedWithExpectedRouteTemplate()
        {
            // Arrange
            MethodInfo? methodInfo = _systemUnderTest.GetType().GetMethod(nameof(_systemUnderTest.GetCalculationByIdAsync));
            string expectedRouteTemplate = DepositCalculatorControllerRoutes.CalculationByIdRoute;

            // Act
            HttpMethodAttribute actualResult = methodInfo.GetAttribute<HttpMethodAttribute>();

            // Assert
            VerifyHttpMethod<HttpGetAttribute>(actualResult, expectedRouteTemplate);
        }

        [Test]
        public void CalculateAsync_HttpPostAttribute_ShouldBeAppliedWithExpectedRouteTemplate()
        {
            // Arrange
            MethodInfo? methodInfo = _systemUnderTest.GetType().GetMethod(nameof(_systemUnderTest.CalculateAsync));
            string expectedRouteTemplate = DepositCalculatorControllerRoutes.DefaultActionRoute;

            // Act
            HttpMethodAttribute actualResult = methodInfo.GetAttribute<HttpMethodAttribute>();

            // Assert
            VerifyHttpMethod<HttpPostAttribute>(actualResult, expectedRouteTemplate);
        }

        [Test]
        public void GetPaginatedCalculationsAsync_PaginationRequestModelValidationActionFilter_ShouldBeApplied()
        {
            // Arrange
            MethodInfo? methodInfo = _systemUnderTest.GetType().GetMethod(nameof(_systemUnderTest.GetPaginatedCalculationsAsync));

            // Act
            TypeFilterAttribute typeFilter = methodInfo.GetAttribute<TypeFilterAttribute>();

            // Assert
            typeFilter
                .Should()
                .NotBeNull();

            typeFilter.ImplementationType
                .Should()
                .BeAssignableTo<ModelValidationActionFilterAttribute<PaginationRequestModel>>();
        }

        [Test]
        public void GetPaginatedCalculationsAsync_EnableCorsAttribute_ShouldBeAppliedWithExpectedPolicyName()
        {
            // Arrange
            MethodInfo? methodInfo = _systemUnderTest.GetType().GetMethod(nameof(_systemUnderTest.GetPaginatedCalculationsAsync));
            string expectedPolicyName = PolicyNames.SpecificPaginationResponseHeaders;

            // Act
            EnableCorsAttribute enableCorsAttribute = methodInfo.GetAttribute<EnableCorsAttribute>();

            // Assert
            enableCorsAttribute
                .Should()
                .NotBeNull();

            enableCorsAttribute.PolicyName
                .Should()
                .Be(expectedPolicyName);
        }

        [Test]
        public void CalculateAsync_CalculationStrategyExceptionFilter_ShouldBeApplied()
        {
            // Arrange
            MethodInfo? methodInfo = _systemUnderTest.GetType().GetMethod(nameof(_systemUnderTest.CalculateAsync));

            // Act
            CalculationStrategyExceptionFilterAttribute calculationStrategyExceptionFilter = methodInfo
                .GetAttribute<CalculationStrategyExceptionFilterAttribute>();

            // Assert
            calculationStrategyExceptionFilter
                .Should()
                .NotBeNull();
        }

        [Test]
        public void CalculateAsync_DepositInfoRequestModelValidationActionFilter_ShouldBeApplied()
        {
            // Arrange
            MethodInfo? methodInfo = _systemUnderTest.GetType().GetMethod(nameof(_systemUnderTest.CalculateAsync));

            // Act
            TypeFilterAttribute typeFilter = methodInfo.GetAttribute<TypeFilterAttribute>();

            // Assert
            typeFilter
                .Should()
                .NotBeNull();

            typeFilter.ImplementationType
                .Should()
                .BeAssignableTo<ModelValidationActionFilterAttribute<DepositInfoRequestModel>>();
        }

        [Test]
        public void DepositCalculatorController_ProducesAttributeWithApplicationJsonMediaType_ShouldBeApplied()
        {
            // Arrange
            string expectedMediaType = MediaTypeNames.Application.Json;

            // Act
            ProducesAttribute actualProducesAttribute = _systemUnderTest.GetType().GetAttribute<ProducesAttribute>();

            // Assert
            actualProducesAttribute
                .Should()
                .NotBeNull();

            actualProducesAttribute.ContentTypes
                .Should()
                .NotBeNull()
                .And
                .Contain(expectedMediaType);
        }

        [Test]
        public void DepositCalculatorController_ProducesResponseType_ExpectedAttributesShouldBeApplied()
        {
            // Arrange
            var expectedResponseStatusCodes = new ProducesResponseTypeAttribute[]
            {
                new ProducesResponseTypeAttribute(StatusCodes.Status200OK),
                new ProducesResponseTypeAttribute(StatusCodes.Status404NotFound),
                new ProducesResponseTypeAttribute(StatusCodes.Status400BadRequest)
            };

            // Act
            IEnumerable<ProducesResponseTypeAttribute> actualResponseStatusCodes = _systemUnderTest
                .GetType()
                .GetAttributes<ProducesResponseTypeAttribute>();

            // Assert
            actualResponseStatusCodes
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(expectedResponseStatusCodes);
        }

        [Test]
        public void CalculateAsync_500InternalServerErrorProducesResponseType_ShouldBeApplied()
        {
            // Arrange
            var expectedResponseStatusCode = new ProducesResponseTypeAttribute(StatusCodes.Status500InternalServerError);
            MethodInfo? methodInfo = _systemUnderTest.GetType().GetMethod(nameof(_systemUnderTest.CalculateAsync));

            // Act
            ProducesResponseTypeAttribute actualResponseStatusCode = methodInfo.GetAttribute<ProducesResponseTypeAttribute>();

            // Assert
            actualResponseStatusCode
                .Should()
                .NotBeNull();

            actualResponseStatusCode
                .Should()
                .BeEquivalentTo(expectedResponseStatusCode);
        }

        #endregion

        #region Action Tests

        [Test]
        public async Task CalculateAsync_DepositInfoRequestModel_ShouldReturnOkObjectWithDepositCalculationResponseModel()
        {
            // Arrange
            var dummyDepositInfoRequestModel = new DepositInfoRequestModel();

            _depositCalculatorOperationsServiceMock
                .Setup(depositCalculatorOperationsService => depositCalculatorOperationsService.CalculateAsync(It.IsAny<DepositInfoRequestModel>()))
                .ReturnsAsync(_fakeDepositCalculationResponseModel);

            var expectedStatusCode = 200;

            // Act
            IActionResult actualResult = await _systemUnderTest.CalculateAsync(dummyDepositInfoRequestModel);

            // Assert
            VerifyObjectResult<OkObjectResult, DepositCalculationResponseModel>(actualResult, expectedStatusCode);
        }

        [Test]
        public async Task GetPaginatedCalculationsAsync_EmptyCalculations_ShouldReturnNotFoundResult()
        {
            // Arrange
            var totalCount = 0;

            var fakePaginatedCalculations = new PaginatedResponseModel<CalculationDetailsResponseModel>(
                totalCount,
                new CalculationDetailsResponseModel[totalCount]);

            SetupGetPaginatedCalculationsAsyncToReturn(fakePaginatedCalculations);

            // Act
            IActionResult result = await _systemUnderTest.GetPaginatedCalculationsAsync(_dummyPaginationRequestModel);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetPaginatedCalculationsAsync_NotEmptyCalculations_ShouldReturnOkObjectAndExpectedResponseHeaders()
        {
            // Arrange
            var totalCount = 34;
            var calculationsCount = 8;

            var expectedResponseHeaders = new HeaderDictionary
            {
                [ResponseHeaderConstants.TotalCount] = totalCount.ToString()
            };

            var fakePaginatedCalculations = new PaginatedResponseModel<CalculationDetailsResponseModel>(
                totalCount,
                new CalculationDetailsResponseModel[calculationsCount]);

            SetupGetPaginatedCalculationsAsyncToReturn(fakePaginatedCalculations);

            var expectedStatusCode = 200;

            // Act
            IActionResult actualResult = await _systemUnderTest.GetPaginatedCalculationsAsync(_dummyPaginationRequestModel);

            // Assert
            VerifyObjectResult<OkObjectResult, CalculationDetailsResponseModel[]>(actualResult, expectedStatusCode);

            _systemUnderTest.Response.Headers
                .Should()
                .BeEquivalentTo(expectedResponseHeaders);
        }

        [Test]
        public async Task GetCalculationByIdAsync_NullDepositCalculationResponseModel_ShouldReturnNotFoundResult()
        {
            // Arrange
            Guid id = Guid.NewGuid();
            var depositCalculationResponseModel = (DepositCalculationResponseModel)null;

            SetupGetCalculationByIdAsyncToReturn(depositCalculationResponseModel);

            // Act
            IActionResult result = await _systemUnderTest.GetCalculationByIdAsync(id);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<NotFoundResult>();
        }

        [Test]
        public async Task GetCalculationByIdAsync_NotNullDepositCalculationResponseModel_ShouldReturnOkObjectWithDepositCalculationResponseModel()
        {
            // Arrange
            SetupGetCalculationByIdAsyncToReturn(_fakeDepositCalculationResponseModel);

            Guid id = Guid.NewGuid();

            var expectedStatusCode = 200;

            // Act
            IActionResult actualResult = await _systemUnderTest.GetCalculationByIdAsync(id);

            // Assert
            VerifyObjectResult<OkObjectResult, DepositCalculationResponseModel>(actualResult, expectedStatusCode);
        }

        #endregion

        private void VerifyObjectResult<TExpectedActionResult, TExpectedValue>(IActionResult result, int expectedStatusCode)
            where TExpectedActionResult : ObjectResult
        {
            result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<TExpectedActionResult>();

            var objectResult = (TExpectedActionResult)result;

            objectResult.StatusCode
                .Should()
                .Be(expectedStatusCode);

            objectResult.Value
                .Should()
                .NotBeNull()
                .And
                .BeOfType<TExpectedValue>();
        }

        private void VerifyHttpMethod<TExpectedHttpMethod>(HttpMethodAttribute httpMethod, string expectedRouteTemplate)
            where TExpectedHttpMethod : HttpMethodAttribute
        {
            httpMethod
                .Should()
                .NotBeNull()
                .And
                .BeOfType<TExpectedHttpMethod>();

            httpMethod.Template
                .Should()
                .Be(expectedRouteTemplate);
        }

        private void SetupGetPaginatedCalculationsAsyncToReturn(PaginatedResponseModel<CalculationDetailsResponseModel> paginatedCalculations)
        {
            _depositCalculatorOperationsServiceMock
                .Setup(depositCalculatorOperationsService => depositCalculatorOperationsService
                    .GetPaginatedCalculationsAsync(It.IsAny<PaginationRequestModel>()))
                .ReturnsAsync(paginatedCalculations);
        }

        private void SetupGetCalculationByIdAsyncToReturn(DepositCalculationResponseModel depositCalculationResponseModel)
        {
            _depositCalculatorOperationsServiceMock
                .Setup(depositCalculatorOperationsService => depositCalculatorOperationsService
                    .GetCalculationByIdAsync(It.IsAny<Guid>()))
                .ReturnsAsync(depositCalculationResponseModel);
        }
    }
}