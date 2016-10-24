using System;
using System.Linq;

namespace ScupTel.Domain.Repositories.PagedResultsGenericRepository
{
    public interface ISortCriteria<T>
    {
        SortDirection Direction { get; set; }

        IQueryable<T> ApplyOrdering(IQueryable<T> query, Boolean useThenBy);
    }
}
