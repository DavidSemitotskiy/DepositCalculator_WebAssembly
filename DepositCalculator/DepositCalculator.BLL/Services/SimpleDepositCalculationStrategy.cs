using System;
using System.Collections.Generic;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Services
{
    /// <inheritdoc cref="IDepositCalculationStrategy" />
    public class SimpleDepositCalculationStrategy : IDepositCalculationStrategy
    {
        /// <inheritdoc />
        public DepositCalculationMethod CalculationMethod => DepositCalculationMethod.Simple;

        /// <inheritdoc />
        public DepositCalculationResponseDTO Calculate(DepositInfoRequestDTO depositInfoRequestDTO)
        {
            decimal roundedDeposit = Math.Round(depositInfoRequestDTO.depositAmount, 2);

            var monthlyDepositCalculationDTOs = new List<MonthlyDepositCalculationResponseDTO>();

            decimal depositByMonth = CalculateByMonth(roundedDeposit, depositInfoRequestDTO.percent);

            var sumDepositByMonths = 0m;

            for (var i = 0; i < depositInfoRequestDTO.periodInMonths; i++)
            {
                sumDepositByMonths += depositByMonth;

                decimal totalDeposit = roundedDeposit + sumDepositByMonths;

                int monthNumber = i + 1;

                var monthlyDepositCalculationDTO = new MonthlyDepositCalculationResponseDTO(
                    monthNumber,
                    sumDepositByMonths,
                    totalDeposit);

                monthlyDepositCalculationDTOs.Add(monthlyDepositCalculationDTO);
            }

            return new DepositCalculationResponseDTO(monthlyDepositCalculationDTOs);
        }

        private decimal CalculateByMonth(decimal depositAmount, decimal percent)
        {
            decimal depositInterestRate = percent / 100;

            var depositPeriodInYears = 1m / 12;

            return depositAmount * depositInterestRate * depositPeriodInYears;
        }
    }
}