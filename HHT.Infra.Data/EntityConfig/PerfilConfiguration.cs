using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class PerfilConfiguration : EntityTypeConfiguration<Perfil>
    {
        public PerfilConfiguration()
        {
            HasKey(p => p.PerfilId);

            Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(25);
        }
    }
}
