using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Dom;
using Bunit;
using DepositCalculator.UI.Components;
using DepositCalculator.UI.Interfaces;
using DepositCalculator.UI.Models;
using DepositCalculator.UI.Utils;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace DepositCalculator.UI.Tests.Components
{
    public class DepositCalculationTableComponentTests : TestContext
    {
        private const string CalculationTableCssSelector = "#scroll";

        private const string HeaderOffsetCssSelector = "#offset";

        private readonly IRenderedComponent<DepositCalculationTableComponent> _systemUnderTest;

        private readonly Mock<IDepositCalculationStateService> _depositCalculationStateServiceMock;

        private readonly List<MonthlyDepositCalculationPageModel> _fakeMonthlyDepositCalculationPageModels;

        private Func<Task> _onChangeAsync;

        public DepositCalculationTableComponentTests()
        {
            _fakeMonthlyDepositCalculationPageModels = new List<MonthlyDepositCalculationPageModel>();
            var fakeDepositCalculationPageModel = new DepositCalculationPageModel(_fakeMonthlyDepositCalculationPageModels);

            _depositCalculationStateServiceMock = new Mock<IDepositCalculationStateService>();

            _depositCalculationStateServiceMock
                .Setup(depositCalculationStateService => depositCalculationStateService.CalculationPageModel)
                .Returns(fakeDepositCalculationPageModel);

            _depositCalculationStateServiceMock
                .SetupAdd(depositCalculationStateService => depositCalculationStateService.OnChangeAsync += It.IsAny<Func<Task>>())
                .Callback((Func<Task> evt) => _onChangeAsync = evt);

            Services.AddSingleton(_depositCalculationStateServiceMock.Object);

            _systemUnderTest = RenderComponent<DepositCalculationTableComponent>();
        }

        [Fact]
        internal void MonthlyCalculations_EmptyCalculations_OnlyHeaderTableShouldBeShown()
        {
            // Arrange
            var expectedMarkup =
                @$"<div id=""calculation-table"">
                       <div class=""header-container"">
                           <table class=""table"">
                               <thead class=""table-head"">
                                   <tr>
                                       <th class=""table-title"">Month</th>
                                       <th class=""table-title"">%</th>
                                       <th class=""table-title"">Sum</th>
                                   </tr>
                               </thead>
                           </table>
                           <div id=""offset"" class={DepositTableStyles.TableHeaderWithoutOffset}></div>
                       </div>
                </div>";

            var cssSelector = ".table-scroll";

            // Act
            Action result = () => _systemUnderTest.Find(cssSelector);

            // Assert
            result
                .Should()
                .Throw<ElementNotFoundException>();

            _systemUnderTest.MarkupMatches(expectedMarkup);
        }

        [Fact]
        internal async Task MonthlyCalculations_NotEmptyCalculations_HeaderTableWithCalculationsShouldBeShown()
        {
            // Arrange
            var fakeMonthlyDepositCalculationPageModels = new MonthlyDepositCalculationPageModel[]
            {
                new MonthlyDepositCalculationPageModel(
                    monthNumber: 2,
                    depositByMonth: 8712m,
                    totalDepositAmount: 91293)
            };

            await AddCalculationsToContextAndRaiseOnChangeEventAsync(fakeMonthlyDepositCalculationPageModels);

            var expectedMarkup =
                @$"<div id=""calculation-table"">
                       <div class=""header-container"">
                           <table class=""table"">
                               <thead class=""table-head"">
                                   <tr>
                                       <th class=""table-title"">Month</th>
                                       <th class=""table-title"">%</th>
                                       <th class=""table-title"">Sum</th>
                                   </tr>
                               </thead>
                           </table>
                           <div id=""offset"" class={DepositTableStyles.TableHeaderWithoutOffset}></div>
                       </div>
                       <div id=""scroll"" class="""">
                           <table class=""table"">
                               <tbody>
                                   <tr>
                                       <td class=""table-term-item table-item "">{_fakeMonthlyDepositCalculationPageModels[0].monthNumber}</td>
                                       <td class=""table-percent-item table-item "">{_fakeMonthlyDepositCalculationPageModels[0].depositByMonth}</td>
                                       <td class=""table-sum-item table-item "">{_fakeMonthlyDepositCalculationPageModels[0].totalDepositAmount}</td>
                                   </tr>
                               </tbody>
                           </table>
                       </div>
                </div>";

            // Act
            Action result = () => _systemUnderTest.Find(CalculationTableCssSelector);

            // Assert
            result
                .Should()
                .NotThrow<ElementNotFoundException>();

            _systemUnderTest.MarkupMatches(expectedMarkup);
        }

        [Fact]
        internal async Task MonthlyCalculations_ContainsCalculations_ShouldHaveRoundedCalculations()
        {
            // Arrange
            var fakeMonthlyDepositCalculationPageModels = new MonthlyDepositCalculationPageModel[]
            {
                new MonthlyDepositCalculationPageModel(
                    monthNumber: 24,
                    depositByMonth: 8712.91282m,
                    totalDepositAmount: 91293.9172563m)
            };

            var expectedMonthlyDepositCalculationPageModels = new MonthlyDepositCalculationPageModel[]
            {
                new MonthlyDepositCalculationPageModel(
                    monthNumber: 24,
                    depositByMonth: 8712.91m,
                    totalDepositAmount: 91293.92m)
            };

            // Act
            await AddCalculationsToContextAndRaiseOnChangeEventAsync(fakeMonthlyDepositCalculationPageModels);

            // Arrange
            _systemUnderTest.Instance.DepositCalculation.monthlyCalculations
                .Should()
                .BeEquivalentTo(expectedMonthlyDepositCalculationPageModels);
        }

        [Fact]
        internal async Task MonthlyCalculations_ContainsLessCalculationsThanCalculationsCountForScroll_ShouldHaveHeaderTableWithoutOffset()
        {
            // Arrange
            var monthlyCalculationsCount = 3;

            IEnumerable<MonthlyDepositCalculationPageModel> fakeMonthlyDepositCalculationPageModels
                = GenerateCalculations(monthlyCalculationsCount);

            await AddCalculationsToContextAndRaiseOnChangeEventAsync(fakeMonthlyDepositCalculationPageModels);

            // Act
            IElement offset = _systemUnderTest.Find(HeaderOffsetCssSelector);

            // Assert
            offset.ClassList
                .Should()
                .Contain(DepositTableStyles.TableHeaderWithoutOffset);
        }

        [Fact]
        internal async Task MonthlyCalculations_ContainsLessCalculationsThanCalculationsCountForScroll_ShouldNotHaveScroll()
        {
            // Arrange
            var monthlyCalculationsCount = 7;

            IEnumerable<MonthlyDepositCalculationPageModel> fakeMonthlyDepositCalculationPageModels
                = GenerateCalculations(monthlyCalculationsCount);

            await AddCalculationsToContextAndRaiseOnChangeEventAsync(fakeMonthlyDepositCalculationPageModels);

            // Act
            IElement calculationsTable = _systemUnderTest.Find(CalculationTableCssSelector);

            // Assert
            calculationsTable.ClassList
                .Should()
                .NotContain(DepositTableStyles.CalculationsTableWithScroll);
        }

        [Fact]
        internal async Task MonthlyCalculations_ContainsMoreThanCalculationsCountForScroll_ShouldHaveHeaderTableWithOffset()
        {
            // Arrange
            int monthlyCalculationsCount = 16;

            IEnumerable<MonthlyDepositCalculationPageModel> fakeMonthlyDepositCalculationPageModels
                = GenerateCalculations(monthlyCalculationsCount);

            await AddCalculationsToContextAndRaiseOnChangeEventAsync(fakeMonthlyDepositCalculationPageModels);

            // Act
            IElement offset = _systemUnderTest.Find(HeaderOffsetCssSelector);

            // Assert
            offset.ClassList
                .Should()
                .Contain(DepositTableStyles.TableHeaderWithOffset);
        }

        [Fact]
        internal async Task MonthlyCalculations_ContainsMoreThanCalculationsCountForScroll_ShouldHaveScroll()
        {
            // Arrange
            int monthlyCalculationsCount = 25;

            IEnumerable<MonthlyDepositCalculationPageModel> fakeMonthlyDepositCalculationPageModels
                = GenerateCalculations(monthlyCalculationsCount);

            await AddCalculationsToContextAndRaiseOnChangeEventAsync(fakeMonthlyDepositCalculationPageModels);

            // Act
            IElement calculationsTable = _systemUnderTest.Find(CalculationTableCssSelector);

            // Assert
            calculationsTable.ClassList
                .Should()
                .Contain(DepositTableStyles.CalculationsTableWithScroll);
        }

        private Task AddCalculationsToContextAndRaiseOnChangeEventAsync(
            IEnumerable<MonthlyDepositCalculationPageModel> calculations)
        {
            _fakeMonthlyDepositCalculationPageModels.AddRange(calculations);

            return _onChangeAsync.Invoke();
        }

        private IEnumerable<MonthlyDepositCalculationPageModel> GenerateCalculations(int count)
        {
            return Enumerable
                .Range(0, count)
                .Select(_ => new MonthlyDepositCalculationPageModel(
                    monthNumber: default,
                    depositByMonth: default,
                    totalDepositAmount: default));
        }
    }
}