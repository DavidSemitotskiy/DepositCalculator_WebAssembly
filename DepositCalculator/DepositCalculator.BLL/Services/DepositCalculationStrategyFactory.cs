using System.Collections.Generic;
using System.Linq;
using DepositCalculator.BLL.Exceptions;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Services
{
    /// <inheritdoc cref="IDepositCalculationStrategyFactory" />
    public class DepositCalculationStrategyFactory : IDepositCalculationStrategyFactory
    {
        private readonly IEnumerable<IDepositCalculationStrategy> _calculationStrategies;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculationStrategyFactory" /> class
        /// using collection of <see cref="IDepositCalculationStrategy" />.
        /// </summary>
        /// <param name="calculationStrategies">
        /// The collection of <see cref="IDepositCalculationStrategy" /> for calculating deposit.
        /// </param>
        public DepositCalculationStrategyFactory(IEnumerable<IDepositCalculationStrategy> calculationStrategies)
        {
            _calculationStrategies = calculationStrategies;
        }

        /// <inheritdoc />
        public IDepositCalculationStrategy Create(DepositCalculationMethod calculationMethod)
        {
            var resolvedStrategy = _calculationStrategies
                .FirstOrDefault(strategy => strategy.CalculationMethod == calculationMethod);

            if (resolvedStrategy == null)
            {
                string errorMessage = string.Format(
                    "There is not such {0} strategy registerred to the container",
                    calculationMethod);

                throw new StrategyNotRegisterredException(errorMessage);
            }

            return resolvedStrategy;
        }
    }
}