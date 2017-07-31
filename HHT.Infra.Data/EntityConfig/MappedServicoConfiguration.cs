using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class MappedServicoConfiguration : EntityTypeConfiguration<MappedServico>
    {
        public MappedServicoConfiguration()
        {
            HasKey(s => s.MappedServicoId);

            Property(s => s.Nome)
                .IsRequired()
                .HasMaxLength(150);

            HasRequired(s => s.Servico)
                .WithMany()
                .HasForeignKey(s => s.ServicoId);
        }
    }
}
