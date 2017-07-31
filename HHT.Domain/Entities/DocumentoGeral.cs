
using System;

namespace HHT.Domain.Entities
{
    public class DocumentoGeral
    {
        public int DocumentoGeralId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Vencimento { get; set; }
        public DateTime DataCadastro { get; set; }

        public int TipoDocumentoId { get; set; }
        public virtual TipoDocumento TipoDocumento { get; set; }

        public int LocalId { get; set; }
        public virtual Local Local { get; set; }

        public int AssociadoId { get; set; }
        public virtual Associado Associado { get; set; }
    }
}
