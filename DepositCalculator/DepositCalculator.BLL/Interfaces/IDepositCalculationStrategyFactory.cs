using DepositCalculator.BLL.Exceptions;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Interfaces
{
    /// <summary>
    /// A contract for creating specific <see cref="IDepositCalculationStrategy" />.
    /// </summary>
    public interface IDepositCalculationStrategyFactory
    {
        /// <summary>
        /// Creates specific <see cref="IDepositCalculationStrategy" />
        /// based on <paramref name="calculationMethod" />.
        /// </summary>
        /// <param name="calculationMethod">The <see cref="DepositCalculationMethod" />.</param>
        /// <returns>Created <see cref="IDepositCalculationStrategy" />.</returns>
        /// <exception cref="StrategyNotRegisterredException">
        /// When requested <see cref="IDepositCalculationStrategy" /> was not provided for
        /// specific <see cref="IDepositCalculationStrategyFactory" />.
        /// </exception>
        IDepositCalculationStrategy Create(DepositCalculationMethod calculationMethod);
    }
}