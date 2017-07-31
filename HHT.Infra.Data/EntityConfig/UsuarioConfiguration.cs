using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class UsuarioConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfiguration()
        {
            HasKey(u => u.UsuarioId);

            Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(60);

            Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(80);

            Property(u => u.Senha)
                .IsRequired()
                .HasMaxLength(60);

            Property(u => u.Perfil)
                .IsOptional()
                .HasMaxLength(20);

            Property(u => u.Porteiro)
                .IsRequired();

            HasRequired(u => u.Empresas)
               .WithMany()
               .HasForeignKey(u => u.EmpresaId);
        }
    }
}
