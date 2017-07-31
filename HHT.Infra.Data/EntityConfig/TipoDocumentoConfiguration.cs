using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class TipoDocumentoConfiguration : EntityTypeConfiguration<TipoDocumento>
    {
        public TipoDocumentoConfiguration()
        {
            HasKey(t => t.TipoDocumentoId);

            Property(t => t.Nome)
                .IsRequired()
                .HasMaxLength(40);
        }
    }
}
