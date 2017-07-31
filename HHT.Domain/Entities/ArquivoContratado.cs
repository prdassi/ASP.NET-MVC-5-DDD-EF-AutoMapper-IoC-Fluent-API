using System;
using System.Collections.Generic;

namespace HHT.Domain.Entities
{
    public class ArquivoContratado
    {
        public ArquivoContratado()
        {
            this.Contratados = new List<Contratado>();
        }

        public int ArquivoContratadoId { get; set; }
        public string Nome { get; set; }
        public string Tamanho { get; set; }
        public string Tipo { get; set; }
        public DateTime? DataDocumento { get; set; }
        public string Comentario { get; set; }
        public DateTime DataCadastro { get; set; }

        public int TipoDocumentoId { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }

        public int LocalId { get; set; }
        public virtual Local Local { get; set; }

        public int DocumentoGeralId { get; set; }
        public virtual DocumentoGeral DocumentoGeral { get; set; }

        public virtual ICollection<Contratado> Contratados { get; set; }
    }
}