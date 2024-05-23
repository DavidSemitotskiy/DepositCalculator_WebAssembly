using AutoMapper;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.API.Mapping
{
    /// <summary>
    /// Provides maps between DTOs and response models that represent deposit information.
    /// </summary>
    public class DepositMappingsProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepositMappingsProfile" /> class and
        /// creates maps between DTOs and response models that represent deposit information.
        /// </summary>
        public DepositMappingsProfile()
        {
            CreateMap<DepositInfoRequestModel, DepositInfoRequestDTO>();

            CreateMap<MonthlyDepositCalculationResponseDTO, MonthlyDepositCalculationResponseModel>();

            CreateMap<DepositCalculationResponseDTO, DepositCalculationResponseModel>();

            CreateMap<CalculationDetailsResponseDTO, CalculationDetailsResponseModel>();

            CreateMap<PaginatedResponseModel<CalculationDetailsResponseDTO>,
                PaginatedResponseModel<CalculationDetailsResponseModel>>();
        }
    }
}