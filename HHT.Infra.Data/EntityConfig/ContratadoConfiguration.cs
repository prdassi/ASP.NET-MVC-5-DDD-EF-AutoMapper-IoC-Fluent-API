using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class ContratadoConfiguration : EntityTypeConfiguration<Contratado>
    {
        public ContratadoConfiguration()
        {
            HasKey(c => c.ContratadoId);

            Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(60);            
            
            Property(c => c.RG)
               .IsRequired()
               .HasMaxLength(15);

            Property(c => c.Ativo)
               .IsRequired();

            Property(c => c.DataAdmissao)
               .IsRequired();           

            Property(c => c.Restricao)
                .IsOptional();

            Property(c => c.ValidadeDocumento)
                .IsOptional();

            Property(c => c.Justificativa)
               .IsOptional()
               .HasMaxLength(300);

            HasRequired(c => c.Empresa)
                .WithMany()
                .HasForeignKey(c => c.EmpresaId);

            HasRequired(c => c.Funcao)
                .WithMany()
                .HasForeignKey(c => c.FuncaoId);

            HasRequired(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId);         
        }
    }
}