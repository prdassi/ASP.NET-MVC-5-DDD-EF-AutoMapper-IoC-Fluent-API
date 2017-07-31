using HHT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace HHT.Infra.Data.EntityConfig
{
    public class ArquivoContratadoConfiguration : EntityTypeConfiguration<ArquivoContratado>
    {
        public ArquivoContratadoConfiguration()
        {
            HasKey(a => a.ArquivoContratadoId);

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

            Property(a => a.Comentario)
                .IsOptional()
                .HasMaxLength(300);

            HasRequired(a => a.Local)
                .WithMany()
                .HasForeignKey(e => e.LocalId);

            HasRequired(a => a.TipoDocumento)
                .WithMany()
                .HasForeignKey(e => e.TipoDocumentoId);

            HasRequired(a => a.DocumentoGeral)
                .WithMany()
                .HasForeignKey(e => e.DocumentoGeralId);
        }
    }
}