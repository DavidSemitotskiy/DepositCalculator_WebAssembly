using Bunit;
using DepositCalculator.UI.Components;

namespace DepositCalculator.UI.Tests.Components
{
    public class HistoryDetailsModalComponentTests : TestContext
    {
        public HistoryDetailsModalComponentTests()
        {
            ComponentFactories.AddStub<DepositCalculationTableComponent>();
        }

        [Fact]
        internal void HistoryDetails_Rendering_ShouldContainExpectedMarkup()
        {
            // Arrange
            var expectedMarkup =
                @"<div class=""history-details-container"">
                      <button class=""custom-btn"">Export to CSV</button>
                  </div>";

            // Act
            IRenderedComponent<HistoryDetailsModalComponent> systemUnderTest
                = RenderComponent<HistoryDetailsModalComponent>();

            // Assert
            systemUnderTest.MarkupMatches(expectedMarkup);
        }
    }
}