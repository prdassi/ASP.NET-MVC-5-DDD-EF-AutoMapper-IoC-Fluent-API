using System.Collections.Generic;

namespace HHT.Domain.Entities
{
    public class Identificacao
    {
        public Identificacao()
        {
            this.Documentos = new List<Documento>();
            this.Locais = new List<LocalIntegracao>();
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
        public List<Documento> Documentos { get; set; }
        public List<LocalIntegracao> Locais { get; set; }

        public class Documento
        {
            public string Nome { get; set; }
            public string Realizado { get; set; }
        }

        public class LocalIntegracao
        {
            public string Sigla { get; set; }
            public string Realizado { get; set; }
        }
    }   
}
