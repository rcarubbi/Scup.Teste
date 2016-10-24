using System.Linq;
using System.Linq.Dynamic;

namespace ScupTel.Domain.Repositories.PagedResultsGenericRepository
{
    public class DynamicFieldSortCriteria<T> : ISortCriteria<T>
    {
        private string _dynamicExpression;

        public DynamicFieldSortCriteria(string dynamicExpression)
        {
            _dynamicExpression = dynamicExpression;
        }

        public IQueryable<T> ApplyOrdering(IQueryable<T> query, bool useThenBy)
        {
            return query.OrderBy(_dynamicExpression);
        }


        public SortDirection Direction
        {
            get;
            set;
        }
    }
}
