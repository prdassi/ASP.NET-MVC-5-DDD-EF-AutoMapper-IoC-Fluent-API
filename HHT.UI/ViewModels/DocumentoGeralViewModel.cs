using System;

namespace HHT.UI.ViewModels
{
    public class DocumentoGeralViewModel
    {
        public int DocumentoGeralId { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public int Vencimento { get; set; }
        public DateTime DataCadastro { get; set; }

        public int TipoDocumentoId { get; set; }
        public virtual TipoDocumentoViewModel TipoDocumento { get; set; }

        public int LocalId { get; set; }
        public virtual LocalViewModel Local { get; set; }

        public int AssociadoId { get; set; }
        public virtual AssociadoViewModel Associado { get; set; }
    }
}