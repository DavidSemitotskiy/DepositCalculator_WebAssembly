using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace DepositCalculator.Shared.Models
{
    /// <summary>
    /// Represents a paginated response model of items.
    /// </summary>
    /// <typeparam name="T">The type of items in the paginated response.</typeparam>
    /// <param name="totalCount">The total count of items across all pages.</param>
    /// <param name="items">The collection of <typeparamref name="T" /> for the current page.</param>
    [ExcludeFromCodeCoverage]
    public record PaginatedResponseModel<T>(
        int totalCount,
        IReadOnlyCollection<T> items) where T : class;
}