using Bunit;
using DepositCalculator.UI.Shared;

namespace DepositCalculator.UI.Tests.Shared
{
    public class MainLayoutTests : TestContext
    {
        public MainLayoutTests()
        {
            ComponentFactories.AddStub<NavMenu>();
        }

        [Fact]
        internal void MainLayoutPage_Rendering_ShouldContainExpectedRenderFragments()
        {
            // Arrange
            var fakeFragment =
                @"<div>
                      <h1>Hldaw</h1>
                </div>";

            var expectedMarkup =
                @"<div class=""layout-container"">
                      <div>
                      </div>
                      <div>
                          <div>
                              <h1>Hldaw</h1>
                          </div>
                      </div>
                </div>";

            // Act
            IRenderedComponent<MainLayout> systemUnderTest = RenderComponent<MainLayout>(parameters => parameters
                .Add(component => component.Body, fakeFragment));

            // Assert
            systemUnderTest.MarkupMatches(expectedMarkup);
        }
    }
}