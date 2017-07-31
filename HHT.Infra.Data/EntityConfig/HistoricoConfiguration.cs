using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    class HistoricoConfiguration : EntityTypeConfiguration<Historico>
    {
        public HistoricoConfiguration()
        {
            HasKey(e => e.HistoricoId);

            Property(e => e.Data)
                .IsRequired();
            
            Property(e => e.Descricao)
               .IsOptional()
               .HasMaxLength(50);

            HasRequired(e => e.Contradado)
                .WithMany()
                .HasForeignKey(e => e.ContratadoId);

            HasRequired(e => e.Usuario)
                .WithMany()
                .HasForeignKey(e => e.UsuarioId);
        }
    }
}
