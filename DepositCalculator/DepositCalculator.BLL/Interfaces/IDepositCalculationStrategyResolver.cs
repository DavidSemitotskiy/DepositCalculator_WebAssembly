using DepositCalculator.BLL.Exceptions;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Interfaces
{
    /// <summary>
    /// A contract for resolving existed <see cref="IDepositCalculationStrategy" />.
    /// </summary>
    public interface IDepositCalculationStrategyResolver
    {
        /// <summary>
        /// Gets existed <see cref="IDepositCalculationStrategy" /> based on
        /// <paramref name="calculationMethod" />.
        /// </summary>
        /// <param name="calculationMethod">
        /// The <see cref="DepositCalculationMethod" /> for which a strategy is to be resolved.
        /// </param>
        /// <returns>Resolved <see cref="IDepositCalculationStrategy" />.</returns>
        /// <exception cref="StrategyNotFoundException">
        /// When no <see cref="IDepositCalculationStrategy" /> was found for requested
        /// <paramref name="calculationMethod" />.
        /// </exception>
        IDepositCalculationStrategy GetStrategy(DepositCalculationMethod calculationMethod);
    }
}