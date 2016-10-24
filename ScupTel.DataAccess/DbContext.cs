using ScupTel.DataAccess.Mappings;
using ScupTel.Domain;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace ScupTel.DataAccess
{
    public class DbContext : System.Data.Entity.DbContext, IDbContext
    {
        public DbContext()
             : base("name=DbContext")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Configurations.Add(new ChargeConfiguration());
        }

        public virtual IDbSet<Charge> Charges
        {
            get; set;
        }

      

        public virtual IDbSet<DiscountPlan> DiscountPlans
        {
            get; set;
        }

        public virtual IDbSet<Zone> Zones
        {
            get; set;
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }

        public virtual new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public void Update<T>(T oldEntity, T newEntity) where T : class
        {
            Entry<T>(oldEntity).CurrentValues.SetValues(newEntity);
            Entry<T>(oldEntity).State = EntityState.Modified;
        }
    }
}
