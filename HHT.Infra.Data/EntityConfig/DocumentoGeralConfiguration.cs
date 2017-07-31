using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class DocumentoGeralConfiguration : EntityTypeConfiguration<DocumentoGeral>
    {
        public DocumentoGeralConfiguration()
        {
            HasKey(d => d.DocumentoGeralId);

            Property(d => d.Nome)
                .IsRequired()
                .HasMaxLength(90);

            Property(d => d.Descricao)
                .IsRequired()
                .HasMaxLength(90);

            Property(d => d.Vencimento)
                .IsRequired();

            HasRequired(d => d.TipoDocumento)
                .WithMany()
                .HasForeignKey(d => d.TipoDocumentoId);

            HasRequired(d => d.Local)
                .WithMany()
                .HasForeignKey(d => d.LocalId);

            HasRequired(d => d.Associado)
                .WithMany()
                .HasForeignKey(d => d.AssociadoId);
        }
    }
}

