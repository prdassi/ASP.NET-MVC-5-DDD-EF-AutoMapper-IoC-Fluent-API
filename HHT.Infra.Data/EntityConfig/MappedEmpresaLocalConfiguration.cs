using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class MappedEmpresaLocalConfiguration : EntityTypeConfiguration<MappedEmpresaLocal>
    {
        public MappedEmpresaLocalConfiguration()
        {
            HasKey(l => l.MappedEmpresaLocalId);

            Property(l => l.Sigla)
                .IsRequired()
                .HasMaxLength(6);

            Property(l => l.Nome)
               .IsRequired()
               .HasMaxLength(30);

            HasRequired(l => l.Local)
                .WithMany()
                .HasForeignKey(l => l.LocalId);
        }
    }
}
