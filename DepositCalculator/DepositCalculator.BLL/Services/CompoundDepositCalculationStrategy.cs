using System;
using System.Collections.Generic;
using DepositCalculator.BLL.DTOs;
using DepositCalculator.BLL.Interfaces;
using DepositCalculator.Shared.Enums;

namespace DepositCalculator.BLL.Services
{
    /// <inheritdoc cref="IDepositCalculationStrategy" />
    public class CompoundDepositCalculationStrategy : IDepositCalculationStrategy
    {
        /// <inheritdoc />
        public DepositCalculationMethod CalculationMethod => DepositCalculationMethod.Compound;

        /// <inheritdoc />
        public DepositCalculationResponseDTO Calculate(DepositInfoRequestDTO depositInfoRequestDTO)
        {
            decimal roundedDeposit = Math.Round(depositInfoRequestDTO.depositAmount, 2);

            var monthlyDepositCalculationDTOs = new List<MonthlyDepositCalculationResponseDTO>();

            decimal depositByMonths = roundedDeposit;

            var interestDeposit = 0m;

            for (var i = 0; i < depositInfoRequestDTO.periodInMonths; i++)
            {
                decimal compoundDeposit = CalculateByMonth(depositByMonths, depositInfoRequestDTO.percent);

                interestDeposit += compoundDeposit - depositByMonths;

                int monthNumber = i + 1;

                var monthlyDepositCalculationDTO = new MonthlyDepositCalculationResponseDTO(
                    monthNumber,
                    interestDeposit,
                    compoundDeposit);

                depositByMonths = compoundDeposit;

                monthlyDepositCalculationDTOs.Add(monthlyDepositCalculationDTO);
            }

            return new DepositCalculationResponseDTO(monthlyDepositCalculationDTOs);
        }

        private decimal CalculateByMonth(decimal depositAmount, decimal percent)
        {
            decimal depositInterestRate = percent / 100;

            var compoundTimes = 12;

            decimal compoundGrow = 1 + (depositInterestRate / compoundTimes);

            return depositAmount * compoundGrow;
        }
    }
}