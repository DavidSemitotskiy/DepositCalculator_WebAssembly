using AutoMapper;
using DepositCalculator.Shared.Models;
using DepositCalculator.UI.Models;

namespace DepositCalculator.UI.Mapping
{
    /// <summary>
    /// Provides maps between response models and page models that represent deposit information.
    /// </summary>
    public class DepositMappingsProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepositMappingsProfile" /> class and
        /// creates maps between response models and page models that represent deposit information.
        /// </summary>
        public DepositMappingsProfile()
        {
            CreateMap<MonthlyDepositCalculationResponseModel, MonthlyDepositCalculationPageModel>();

            CreateMap<DepositCalculationResponseModel, DepositCalculationPageModel>();

            CreateMap<CalculationDetailsResponseModel, CalculationDetailsPageModel>();
        }
    }
}