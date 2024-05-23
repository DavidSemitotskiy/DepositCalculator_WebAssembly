using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DepositCalculator.DAL.Interfaces;
using DepositCalculator.DAL.Services;
using DepositCalculator.Entities;
using DepositCalculator.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace DepositCalculator.DAL.Repositories
{
    /// <inheritdoc cref="IDepositCalculationRepository" />
    public class DepositCalculationRepository : BaseRepository<DepositCalculationEntity, Guid>, IDepositCalculationRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DepositCalculationRepository" /> class
        /// using a wrapper around <see cref="DepositCalculatorContext" />.
        /// </summary>
        /// <param name="databaseContext">
        /// The wrapper around <see cref="DepositCalculatorContext" /> used for database interaction.
        /// </param>
        public DepositCalculationRepository(IDbContextWrapper<DepositCalculatorContext> databaseContext)
            : base(databaseContext)
        {
        }

        /// <inheritdoc />
        public override Task<DepositCalculationEntity?> GetByIdAsync(Guid id)
        {
            return _entities
                .AsNoTracking()
                .Include(depositCalculation => depositCalculation.MonthlyCalculations)
                .FirstOrDefaultAsync(depositCalculation => depositCalculation.Id == id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<DepositCalculationEntity>> GetByPageAsync(PaginationRequestModel paginationRequest)
        {
            return await _entities
                .AsNoTracking()
                .OrderBy(depositCalculation => depositCalculation.CalculatedAt)
                .Skip((paginationRequest.PageNumber - 1) * paginationRequest.PageSize)
                .Take(paginationRequest.PageSize)
                .ToListAsync();
        }
    }
}