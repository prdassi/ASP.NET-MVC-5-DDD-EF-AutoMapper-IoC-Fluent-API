using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class ServicoConfiguration : EntityTypeConfiguration<Servico>
    {
        public ServicoConfiguration()
        {
            HasKey(s => s.ServicoId);

            Property(s => s.Nome)
                .IsRequired()
                .HasMaxLength(150);           
        }
    }
}
