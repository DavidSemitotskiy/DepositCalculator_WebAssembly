using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.DAL.Interfaces;
using DepositCalculator.Entities;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.BLL.Services
{
    /// <inheritdoc cref="IDepositCalculatorService" />
    public class DepositCalculatorService : IDepositCalculatorService
    {
        private readonly IMapper _mapper;

        private readonly IDepositCalculationStrategyResolver _strategyResolver;

        private readonly IDepositCalculationRepository _depositCalculationRepository;

        private readonly IDateTimeWrapper _time;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculatorService" />
        /// class using <see cref="IMapper" />, <see cref="IDepositCalculationStrategyResolver" />,
        /// <see cref="IDepositCalculationRepository" /> and <see cref="IDateTimeWrapper" />.
        /// </summary>
        /// <param name="mapper">The <see cref="IMapper" /> for mapping between DTOs and Entities.</param>
        /// <param name="strategyResolver">
        /// The <see cref="IDepositCalculationStrategyResolver" /> for resolving existed
        /// <see cref="IDepositCalculationStrategy" />.
        /// </param>
        /// <param name="depositCalculationRepository">
        /// The <see cref="IDepositCalculationRepository" /> for operations with database.
        /// </param>
        /// <param name="time">The <see cref="IDateTimeWrapper" /> for retrieving date and time.</param>
        public DepositCalculatorService(
            IMapper mapper,
            IDepositCalculationStrategyResolver strategyResolver,
            IDepositCalculationRepository depositCalculationRepository,
            IDateTimeWrapper time)
        {
            _mapper = mapper;
            _strategyResolver = strategyResolver;
            _depositCalculationRepository = depositCalculationRepository;
            _time = time;
        }

        /// <inheritdoc />
        public async Task<DepositCalculationResponseDTO> CalculateAsync(DepositInfoRequestDTO depositInfoRequestDTO)
        {
            IDepositCalculationStrategy calculationStrategy = _strategyResolver
                .GetStrategy(depositInfoRequestDTO.calculationMethod);

            DepositCalculationResponseDTO calculationDTO = calculationStrategy.Calculate(depositInfoRequestDTO);

            DepositCalculationEntity calculationEntity = _mapper.Map<DepositCalculationEntity>(calculationDTO);
            calculationEntity = _mapper.Map(depositInfoRequestDTO, calculationEntity);
            calculationEntity.CalculatedAt = _time.Now;

            await _depositCalculationRepository.AddAsync(calculationEntity);

            return calculationDTO;
        }

        /// <inheritdoc />
        public async Task<PaginatedResponseModel<CalculationDetailsResponseDTO>> GetPaginatedCalculationsAsync(
            PaginationRequestModel paginationRequest)
        {
            IEnumerable<DepositCalculationEntity> paginatedCalculations = await _depositCalculationRepository
                .GetByPageAsync(paginationRequest);

            int totalCount = await _depositCalculationRepository.CountAsync();

            IReadOnlyCollection<CalculationDetailsResponseDTO> paginatedCalculationDetails = _mapper
                .Map<IReadOnlyCollection<CalculationDetailsResponseDTO>>(paginatedCalculations);

            return new PaginatedResponseModel<CalculationDetailsResponseDTO>(
                totalCount,
                paginatedCalculationDetails);
        }

        /// <inheritdoc />
        public async Task<DepositCalculationResponseDTO?> GetCalculationByIdAsync(Guid calculationId)
        {
            DepositCalculationEntity? calculationEntity = await _depositCalculationRepository.GetByIdAsync(calculationId);

            DepositCalculationResponseDTO? calculationDTO = _mapper.Map<DepositCalculationResponseDTO?>(calculationEntity);

            return calculationDTO;
        }
    }
}