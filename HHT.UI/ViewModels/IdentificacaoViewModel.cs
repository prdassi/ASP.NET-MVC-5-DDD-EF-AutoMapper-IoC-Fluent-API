using System.Collections.Generic;

namespace HHT.UI.ViewModels
{
    public class IdentificacaoViewModel
    {
        public IdentificacaoViewModel()
        {
            this.Documentos = new List<DocumentoViewModel>();
            this.Locais = new List<LocalIntegracaoViewModel>();
        }
        
        public string AcessoPermitido { get; set; }
        public string Empresa { get; set; }
        public string Colaborador { get; set; }
        public string Funcao { get; set; }
        public string RG { get; set; }
        public string IntegracaoCompleta { get; set; }
        public string ASO { get; set; }
        public string AprovadoPor { get; set; }
        public string Data { get; set; }
        public string QRCode { get; set; }
        public List<DocumentoViewModel> Documentos { get; set; }
        public List<LocalIntegracaoViewModel> Locais { get; set; }

        public class DocumentoViewModel
        {
            public string Nome { get; set; }
            public string Realizado { get; set; }
        }

        public class LocalIntegracaoViewModel
        {
            public string Sigla { get; set; }
            public string Realizado { get; set; }
        }
    }
    
}