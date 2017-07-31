using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class LocalConfiguration : EntityTypeConfiguration<Local>
    {
        public LocalConfiguration()
        {
            HasKey(l => l.LocalId);

            Property(l => l.Sigla)
                .IsRequired()
                .HasMaxLength(6);

            Property(l => l.Nome)
               .IsRequired()
               .HasMaxLength(30);

           
        }
    }
}
