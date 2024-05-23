using AutoMapper;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.Entities;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.BLL.Mapping
{
    /// <summary>
    /// Provides maps between DTOs and Entities that represent deposit information.
    /// </summary>
    public class DepositMappingsProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepositMappingsProfile" /> class and
        /// creates maps between DTOs and Entities that represent deposit information.
        /// </summary>
        public DepositMappingsProfile()
        {
            CreateMap<MonthlyDepositCalculationResponseDTO, MonthlyDepositCalculationEntity>(MemberList.Source)
                .ReverseMap();

            CreateMap<DepositCalculationResponseDTO, DepositCalculationEntity>(MemberList.Source)
                .ReverseMap();

            CreateMap<DepositInfoRequestDTO, DepositCalculationEntity>(MemberList.Source)
                .ForSourceMember(source => source.calculationMethod, options => options.DoNotValidate());

            CreateMap<DepositCalculationEntity, CalculationDetailsResponseDTO>();

            CreateMap<PaginatedResponseModel<DepositCalculationEntity>,
                PaginatedResponseModel<CalculationDetailsResponseDTO>>();
        }
    }
}