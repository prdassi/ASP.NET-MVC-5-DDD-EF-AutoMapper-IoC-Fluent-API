using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class ArquivoEmpresaConfiguration : EntityTypeConfiguration<ArquivoEmpresa>
    {
        public ArquivoEmpresaConfiguration()
        {
            HasKey(a => a.ArquivoEmpresaId);

            Property(a => a.Tamanho)
                .IsRequired()
                .HasMaxLength(10);

            Property(a => a.Tipo)
                .IsRequired()
                .HasMaxLength(20);

            Property(a => a.Nome)
                .IsRequired()
                .HasMaxLength(200);

            Property(a => a.DataDocumento)
                .IsRequired();

            HasRequired(e => e.DocumentosGeral)
                .WithMany()
                .HasForeignKey(e => e.DocumentoGeralId);
        }
    }
}
