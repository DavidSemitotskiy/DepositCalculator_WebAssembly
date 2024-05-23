using System.Collections.Generic;
using DepositCalculator.BLL.Exceptions;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Services
{
    /// <inheritdoc cref="IDepositCalculationStrategyResolver" />
    public class DepositCalculationStrategyResolver : IDepositCalculationStrategyResolver
    {
        private readonly IDictionary<DepositCalculationMethod, IDepositCalculationStrategy> _calculationStrategies;

        private readonly IDepositCalculationStrategyFactory _strategyFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculationStrategyResolver" /> class using
        /// <see cref="IDepositCalculationStrategyFactory" />.
        /// </summary>
        /// <param name="strategyFactory">
        /// The <see cref="IDepositCalculationStrategyFactory" /> for creating <see cref="IDepositCalculationStrategy" />.
        /// </param>
        public DepositCalculationStrategyResolver(IDepositCalculationStrategyFactory strategyFactory)
        {
            _strategyFactory = strategyFactory;
            _calculationStrategies = InitializeStrategies();
        }

        /// <inheritdoc />
        public IDepositCalculationStrategy GetStrategy(DepositCalculationMethod calculationMethod)
        {
            _calculationStrategies.TryGetValue(calculationMethod, out IDepositCalculationStrategy? calculationStrategy);

            if (calculationStrategy == null)
            {
                string errorMessage = string.Format(
                    "There is not such {0} strategy",
                    calculationMethod);

                throw new StrategyNotFoundException(errorMessage);
            }

            return calculationStrategy;
        }

        private IDictionary<DepositCalculationMethod, IDepositCalculationStrategy> InitializeStrategies()
        {
            return new Dictionary<DepositCalculationMethod, IDepositCalculationStrategy>
            {
                [DepositCalculationMethod.Simple] = _strategyFactory.Create(DepositCalculationMethod.Simple),
                [DepositCalculationMethod.Compound] = _strategyFactory.Create(DepositCalculationMethod.Compound)
            };
        }
    }
}