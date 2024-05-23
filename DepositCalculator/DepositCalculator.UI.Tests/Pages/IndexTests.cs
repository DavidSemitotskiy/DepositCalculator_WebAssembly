using Bunit;
using Castle.Core.Internal;
using DepositCalculator.UI.Components;
using DepositCalculator.UI.Pages;
using Microsoft.AspNetCore.Components;

namespace DepositCalculator.UI.Tests.Pages
{
    public class IndexTests : TestContext
    {
        private readonly IRenderedComponent<Index> _systemUnderTest;

        public IndexTests()
        {
            ComponentFactories.AddStub<DepositFormComponent>();
            ComponentFactories.AddStub<DepositCalculationTableComponent>();

            _systemUnderTest = RenderComponent<Index>();
        }

        [Fact]
        internal void Index_RouteAttribute_ShouldBeValidRoute()
        {
            // Arrange
            var expectedRoute = "/";

            // Act
            RouteAttribute actualRouteAttribute = _systemUnderTest.Instance
                .GetType()
                .GetAttribute<RouteAttribute>();

            // Assert
            actualRouteAttribute
                .Should()
                .NotBeNull();

            actualRouteAttribute.Template
                .Should()
                .Be(expectedRoute);
        }

        [Fact]
        internal void IndexPage_Rendering_ShouldHaveExpectedMarkup()
        {
            // Arrange
            var expectedMarkup =
                @"<div class=""container"">
                      <div class=""form-container""></div>
                      <div class=""table-container""></div>
                </div>";

            // Act

            // Assert
            _systemUnderTest.MarkupMatches(expectedMarkup);
        }
    }
}