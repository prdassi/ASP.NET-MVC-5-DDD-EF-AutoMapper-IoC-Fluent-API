using System;
using System.Collections.Generic;

namespace HHT.UI.ViewModels
{
    public class ArquivoContratadoViewModel
    {
        public ArquivoContratadoViewModel()
        {
            this.Contratados = new List<ContratadoViewModel>();
        }

        public int ArquivoContratadoId { get; set; }
        public string Nome { get; set; }
        public string Tamanho { get; set; }
        public string Tipo { get; set; }
        public DateTime? DataDocumento { get; set; }
        public string Comentario { get; set; }
        public DateTime DataCadastro { get; set; }

        public int TipoDocumentoId { get; set; }
        public virtual TipoDocumentoViewModel TipoDocumento { get; set; }

        public int LocalId { get; set; }
        public virtual LocalViewModel Local { get; set; }

        public int DocumentoGeralId { get; set; }
        public virtual DocumentoGeralViewModel DocumentoGeral { get; set; }

        public virtual ICollection<ContratadoViewModel> Contratados { get; set; }
    }
}