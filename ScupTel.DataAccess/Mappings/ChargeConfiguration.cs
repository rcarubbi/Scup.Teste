using ScupTel.Domain;
using System.Data.Entity.ModelConfiguration;

namespace ScupTel.DataAccess.Mappings
{
    public class ChargeConfiguration : EntityTypeConfiguration<Charge>
    {
        public ChargeConfiguration()
        {
            HasOptional(c => c.From);
            HasOptional(c => c.To);
        }

    }
}
