using System;

namespace HHT.Domain.Entities
{
    public class TipoDocumento
    {
        public int TipoDocumentoId { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
