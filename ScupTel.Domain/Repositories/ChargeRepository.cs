using ScupTel.Domain.Repositories.PagedResultsGenericRepository;
using System.Linq;

namespace ScupTel.Domain.Repositories
{
    public class ChargeRepository : GenericRepository<Charge>
    {
        public ChargeRepository(IDbContext context)
            : base(context)
        {

        }

        public Charge FindByZones(int sourceCode, int targetCode)
        {
            SearchQuery<Charge> query = new SearchQuery<Charge>();
            query.AddFilter(c => c.From.Code == sourceCode && c.To.Code == targetCode);
            
            return Search(query).Entities.FirstOrDefault();
        }
    }
}
