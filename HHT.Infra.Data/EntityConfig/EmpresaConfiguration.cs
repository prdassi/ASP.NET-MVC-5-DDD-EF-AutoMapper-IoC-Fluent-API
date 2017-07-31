using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class EmpresaConfiguration : EntityTypeConfiguration<Empresa>
    {
        public EmpresaConfiguration()
        {

            HasKey(e => e.EmpresaId);

            Property(e => e.Nome)
                .IsRequired()
                .HasMaxLength(150);

            Property(e => e.CNPJ)
               .IsRequired()
               .HasMaxLength(25);
            
            Property(e => e.NomeGestorAES)
               .IsOptional()
               .HasMaxLength(60);

            Property(e => e.EmailGestorAES)
               .IsOptional()
               .HasMaxLength(80);

            Property(e => e.NomeResponsavelEmpresa)
               .IsRequired()
               .HasMaxLength(60);

            Property(e => e.EmailResponsavelEmpresa)
               .IsRequired()
               .HasMaxLength(80);

            Property(e => e.NomeMonitorAES)
               .IsOptional()
               .HasMaxLength(60);

            Property(e => e.EmailMonitorAES)
               .IsOptional()
               .HasMaxLength(80);

            Property(e => e.ValidadeDocumento)
                .IsOptional();

            Property(e => e.VerificarArquivo)
                .IsOptional()
                .HasMaxLength(10);

            Property(e => e.Contrato)
                .IsRequired();         
        }
    }
}
