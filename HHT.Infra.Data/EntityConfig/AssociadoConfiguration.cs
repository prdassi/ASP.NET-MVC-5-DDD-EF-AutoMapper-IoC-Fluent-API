using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class AssociadoConfiguration : EntityTypeConfiguration<Associado>
    {
        public AssociadoConfiguration()
        {
            HasKey(a => a.AssociadoId);

            Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(40);
        }
    }
}
