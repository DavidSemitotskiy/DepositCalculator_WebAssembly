using DepositCalculator.BLL.DTOs;
using DepositCalculator.Shared.Enums;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.API.Tests.Mapping
{
    internal class DepositInfoRequestDTOMappingTests : BaseMappingTests
    {
        [Test]
        public void Mapping_DepositInfoRequestModel_ShouldReturnMappedDepositInfoRequestDTO()
        {
            // Arrange
            var depositInfoRequestModel = new DepositInfoRequestModel
            {
                DepositAmount = 1291,
                Percent = 12,
                PeriodInMonths = 31,
                CalculationMethod = DepositCalculationMethod.Simple
            };

            // Act
            DepositInfoRequestDTO result = _mapper.Map<DepositInfoRequestDTO>(depositInfoRequestModel);

            // Assert
            result
                .Should()
                .NotBeNull()
                .And
                .BeEquivalentTo(depositInfoRequestModel, options => options
                    .WithMapping(
                        nameof(DepositInfoRequestModel.DepositAmount),
                        nameof(DepositInfoRequestDTO.depositAmount))
                    .WithMapping(
                        nameof(DepositInfoRequestModel.CalculationMethod),
                        nameof(DepositInfoRequestDTO.calculationMethod))
                    .WithMapping(
                        nameof(DepositInfoRequestModel.Percent),
                        nameof(DepositInfoRequestDTO.percent))
                    .WithMapping(
                        nameof(DepositInfoRequestModel.PeriodInMonths),
                        nameof(DepositInfoRequestDTO.periodInMonths)));
        }
    }
}