using dept_croatia.Infrastructure.Filters;
using dept_croatia.Infrastructure.Models;

namespace dept_croatia.Infrastructure.Contracts
{
    public interface ISearchService
    {
        Task<List<AggregatedResult>> GetSearchResult(MovieDbFilters filters, int page = 1, int pageSize = 10);
    }
}
