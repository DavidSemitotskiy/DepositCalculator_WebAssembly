using DepositCalculator.UI.Components;

namespace DepositCalculator.UI.Utils
{
    /// <summary>
    /// Constants of style names for <see cref="DepositCalculationTableComponent" />.
    /// </summary>
    public static class DepositTableStyles
    {
        /// <summary>
        /// The name of style for <see cref="DepositCalculationTableComponent" /> header with offset.
        /// </summary>
        public const string TableHeaderWithOffset = "scroll-div";

        /// <summary>
        /// The name of style for <see cref="DepositCalculationTableComponent" /> header without offset.
        /// </summary>
        public const string TableHeaderWithoutOffset = "invisible-div";

        /// <summary>
        /// The name of style for <see cref="DepositCalculationTableComponent" /> calculations body
        /// with scroll.
        /// </summary>
        public const string CalculationsTableWithScroll = "table-scroll";
    }
}