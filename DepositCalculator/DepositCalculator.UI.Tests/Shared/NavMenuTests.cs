using Bunit;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Shared;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DepositCalculator.UI.Tests.Shared
{
    public class NavMenuTests : TestContext
    {
        private readonly IRenderedComponent<NavMenu> _systemUnderTest;

        private readonly Mock<IDepositCalculationStateService> _depositCalculationStateServiceMock;

        public NavMenuTests()
        {
            _depositCalculationStateServiceMock = new Mock<IDepositCalculationStateService>();

            Services.AddSingleton(_depositCalculationStateServiceMock.Object);

            _systemUnderTest = RenderComponent<NavMenu>();
        }

        [Fact]
        public void Navigate_NavigatingToAnotherPage_ShouldClearDepositCalculationStateService()
        {
            // Arrange
            var linkCssSelector = ".nav-link";

            // Act
            _systemUnderTest.Find(linkCssSelector).Click();

            // Assert
            _depositCalculationStateServiceMock.Verify(
                depositCalculationStateService => depositCalculationStateService.ClearDepositCalculationPageModel(),
                Times.Once);
        }
    }
}