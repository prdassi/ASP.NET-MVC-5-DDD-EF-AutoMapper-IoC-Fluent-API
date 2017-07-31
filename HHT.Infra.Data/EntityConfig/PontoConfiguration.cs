using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class PontoConfiguration : EntityTypeConfiguration<Ponto>
    {
        public PontoConfiguration()
        {
            HasKey(p => p.PontoId);

            Property(p => p.DataRegistro)
                .IsRequired();

            Property(p => p.HoraRegistro)
                .IsOptional();

            Property(p => p.Registro)
                .IsRequired()
                .HasMaxLength(15);

            Property(p => p.ModoRegistro)
                .IsRequired()
                .HasMaxLength(15);

            Property(p => p.Justificativa)
               .IsOptional()
               .HasMaxLength(300);

            HasRequired(p => p.Usuario)
               .WithMany()
               .HasForeignKey(p => p.UsuarioId);

            HasRequired(p => p.Contratado)
                .WithMany()
                .HasForeignKey(p => p.ContratadoId);

            HasRequired(p => p.Local)
                .WithMany()
                .HasForeignKey(p => p.LocalId);
        }
    }
}