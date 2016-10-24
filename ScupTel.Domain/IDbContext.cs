using System.Data.Entity;

namespace ScupTel.Domain
{
    public interface IDbContext
    {
        void SaveChanges();

        void Update<T>(T oldValue, T newValue) where T : class;

        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        IDbSet<Zone> Zones { get; set; }

        IDbSet<DiscountPlan> DiscountPlans { get; set; }

        IDbSet<Charge> Charges { get; set; }
    }
}
