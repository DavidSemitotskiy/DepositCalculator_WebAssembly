using System;
using System.Threading.Tasks;
using AutoMapper;
using DepositCalculator.API.Interfaces;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.API.Services
{
    /// <inheritdoc cref="IDepositCalculatorOperationsService" />
    public class DepositCalculatorOperationsService : IDepositCalculatorOperationsService
    {
        private readonly IDepositCalculatorService _depositCalculatorService;

        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculatorOperationsService" /> class using
        /// <see cref="IDepositCalculatorService" /> and <see cref="IMapper" />.
        /// </summary>
        /// <param name="depositCalculatorService">The <see cref="IDepositCalculatorService" /> for operations on deposit.</param>
        /// <param name="mapper">The <see cref="IMapper" /> for mapping between DTOs and response models.</param>
        public DepositCalculatorOperationsService(
            IDepositCalculatorService depositCalculatorService,
            IMapper mapper)
        {
            _depositCalculatorService = depositCalculatorService;
            _mapper = mapper;
        }

        /// <inheritdoc />
        public async Task<DepositCalculationResponseModel> CalculateAsync(DepositInfoRequestModel depositInfoRequestModel)
        {
            DepositInfoRequestDTO depositInfoRequest = _mapper.Map<DepositInfoRequestDTO>(depositInfoRequestModel);

            DepositCalculationResponseDTO calculationDTO = await _depositCalculatorService.CalculateAsync(depositInfoRequest);

            DepositCalculationResponseModel calculationResponse = _mapper.Map<DepositCalculationResponseModel>(calculationDTO);

            return calculationResponse;
        }

        /// <inheritdoc />
        public async Task<PaginatedResponseModel<CalculationDetailsResponseModel>> GetPaginatedCalculationsAsync(
            PaginationRequestModel paginationRequest)
        {
            PaginatedResponseModel<CalculationDetailsResponseDTO> paginatedCalculationDTOs = await _depositCalculatorService
                .GetPaginatedCalculationsAsync(paginationRequest);

            PaginatedResponseModel<CalculationDetailsResponseModel> paginatedCalculationsResponse = _mapper
                .Map<PaginatedResponseModel<CalculationDetailsResponseModel>>(paginatedCalculationDTOs);

            return paginatedCalculationsResponse;
        }

        /// <inheritdoc />
        public async Task<DepositCalculationResponseModel?> GetCalculationByIdAsync(Guid calculationId)
        {
            DepositCalculationResponseDTO? calculationDTO = await _depositCalculatorService.GetCalculationByIdAsync(calculationId);

            DepositCalculationResponseModel? calculationResponse = _mapper.Map<DepositCalculationResponseModel?>(calculationDTO);

            return calculationResponse;
        }
    }
}