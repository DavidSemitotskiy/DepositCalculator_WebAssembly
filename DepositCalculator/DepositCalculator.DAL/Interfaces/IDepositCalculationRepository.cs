using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DepositCalculator.Entities;
using DepositCalculator.Shared.Models;

namespace DepositCalculator.DAL.Interfaces
{
    /// <summary>
    /// A repository contract for providing specific operations for <see cref="DepositCalculationEntity" />.
    /// </summary>
    public interface IDepositCalculationRepository : IBaseRepository<DepositCalculationEntity, Guid>
    {
        /// <summary>
        /// Retrieves a paginated collection of <see cref="DepositCalculationEntity" /> based on
        /// <paramref name="paginationRequest" />.
        /// </summary>
        /// <param name="paginationRequest">
        /// The <see cref="PaginationRequestModel" /> that provides data for pagination.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous get by page operation. The task result contains the paginated
        /// collection of <see cref="DepositCalculationEntity" /> from database.
        /// </returns>
        Task<IEnumerable<DepositCalculationEntity>> GetByPageAsync(PaginationRequestModel paginationRequest);
    }
}