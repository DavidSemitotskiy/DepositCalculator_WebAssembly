using DepositCalculator.API.Filters;
using DepositCalculator.Shared.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;

namespace DepositCalculator.API.Tests.Filters
{
    [TestFixture(typeof(DepositInfoRequestModel))]
    internal class ModelValidationActionFilterAttributeTests<T> : BaseFilterAttributeTests where T : class, new()
    {
        private ModelValidationActionFilterAttribute<T> _systemUnderTest;

        private ValidationResult _fakeValidationResult;

        private Mock<IValidator<T>> _validatorMock;

        private ActionExecutingContext _fakeActionExecutingContext;

        private IDictionary<string, object> _fakeArguments;

        [SetUp]
        public void Setup()
        {
            _fakeValidationResult = new ValidationResult();

            _validatorMock = new Mock<IValidator<T>>();

            _validatorMock
                .Setup(validator => validator.Validate(It.IsAny<T>()))
                .Returns(_fakeValidationResult);

            _fakeArguments = new Dictionary<string, object>();

            var filterMetadata = new List<IFilterMetadata>();

            _fakeActionExecutingContext = new ActionExecutingContext(
                _fakeActionContext,
                filterMetadata,
                _fakeArguments,
                controller: null);

            _systemUnderTest = new ModelValidationActionFilterAttribute<T>(_validatorMock.Object);
        }

        [Test]
        public void OnActionExecuting_EmptyArguments_ShouldNotValidate()
        {
            // Arrange

            // Act
            _systemUnderTest.OnActionExecuting(_fakeActionExecutingContext);

            // Assert
            _validatorMock.Verify(validator => validator.Validate(It.IsAny<T>()), Times.Never);
        }

        [Test]
        public void OnActionExecuting_InValidArgument_ShouldNotValidate()
        {
            // Arrange
            _fakeArguments["argument"] = 23;

            // Act
            _systemUnderTest.OnActionExecuting(_fakeActionExecutingContext);

            // Assert
            _validatorMock.Verify(validator => validator.Validate(It.IsAny<T>()), Times.Never);
        }

        [Test]
        public void OnActionExecuting_NullArgument_ShouldNotValidate()
        {
            // Arrange
            _fakeArguments["argument"] = (T)null;

            // Act
            _systemUnderTest.OnActionExecuting(_fakeActionExecutingContext);

            // Assert
            _validatorMock.Verify(validator => validator.Validate(It.IsAny<T>()), Times.Never);
        }

        [Test]
        public void OnActionExecuting_ValidArgumentAsFirstParameter_ShouldValidate()
        {
            // Arrange
            _fakeArguments["argument"] = new T();

            // Act
            _systemUnderTest.OnActionExecuting(_fakeActionExecutingContext);

            // Assert
            _validatorMock.Verify(validator => validator.Validate(It.IsAny<T>()), Times.Once);
        }

        [Test]
        public void OnActionExecuting_ValidArgumentAsNotFirstParameter_ShouldValidate()
        {
            // Arrange
            _fakeArguments["argument1"] = 54;
            _fakeArguments["argument2"] = new T();

            // Act
            _systemUnderTest.OnActionExecuting(_fakeActionExecutingContext);

            // Assert
            _validatorMock.Verify(validator => validator.Validate(It.IsAny<T>()), Times.Once);
        }

        [Test]
        public void OnActionExecuting_ValidModel_ShouldNotSetActionExecutingContextResult()
        {
            // Arrange
            _fakeArguments["argument"] = new T();

            // Act
            _systemUnderTest.OnActionExecuting(_fakeActionExecutingContext);

            // Assert
            _fakeActionExecutingContext.Result
                .Should()
                .BeNull();
        }

        [Test]
        public void OnActionExecuting_InValidModel_ShouldSetActionExecutingContextResultWithBadRequestObjectResult()
        {
            // Arrange
            _fakeArguments["argument"] = new T();

            _fakeValidationResult.Errors.Add(new ValidationFailure(
                propertyName: "Property",
                errorMessage: "Error"));

            // Act
            _systemUnderTest.OnActionExecuting(_fakeActionExecutingContext);

            // Assert
            _fakeActionExecutingContext.Result
                .Should()
                .NotBeNull()
                .And
                .BeOfType<BadRequestObjectResult>();

            var badRequestObjectResult = _fakeActionExecutingContext.Result as BadRequestObjectResult;

            badRequestObjectResult.Value
                .Should()
                .BeEquivalentTo(_fakeValidationResult.ToDictionary());
        }
    }
}