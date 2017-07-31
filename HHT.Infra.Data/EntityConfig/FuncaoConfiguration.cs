using HHT.Domain.Entities;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class FuncaoConfiguration : EntityTypeConfiguration<Funcao>
    {
        public FuncaoConfiguration()
        {
            HasKey(f => f.FuncaoId);

            Property(f => f.Nome)
                .IsRequired()
                .HasMaxLength(60);
        }
    }
}
