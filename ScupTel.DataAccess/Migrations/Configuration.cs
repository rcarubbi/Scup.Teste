namespace ScupTel.DataAccess.Migrations
{
    using Domain;
    using System.Linq;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<ScupTel.DataAccess.DbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(ScupTel.DataAccess.DbContext context)
        {
            SeedZones(context);
            SeedPlans(context);
            SeedCharges(context);
        }

        private void SeedPlans(DbContext context)
        {
            context.Zones.AddOrUpdate(z => z.Name, new Zone("São Paulo (011)", 11),
             new Zone("São Paulo (016)", 16),
             new Zone("São Paulo (017)", 17),
             new Zone("São Paulo (018)", 18));
            context.SaveChanges();
        }

        private void SeedCharges(DbContext context)
        {
            context.Charges.AddOrUpdate(c => c.Id,
            new Charge(context.Zones.Single(z => z.Code == 11), context.Zones.Single(z => z.Code == 16), 1.9M),
                new Charge(context.Zones.Single(z => z.Code == 16), context.Zones.Single(z => z.Code == 11), 2.9M),
                new Charge(context.Zones.Single(z => z.Code == 11), context.Zones.Single(z => z.Code == 17), 1.7M),
                new Charge(context.Zones.Single(z => z.Code == 17), context.Zones.Single(z => z.Code == 11), 2.7M),
                new Charge(context.Zones.Single(z => z.Code == 11), context.Zones.Single(z => z.Code == 18), 0.9M),
                new Charge(context.Zones.Single(z => z.Code == 18), context.Zones.Single(z => z.Code == 11), 1.9M));
            context.SaveChanges();
        }

        private void SeedZones(DbContext context)
        {
            context.DiscountPlans.AddOrUpdate(z => z.Name,
                new DiscountPlan("Fale Mais 30", 30),
                new DiscountPlan("Fale Mais 60", 60),
                new DiscountPlan("Fale Mais 120", 120));
            context.SaveChanges();
        }
    }
}
