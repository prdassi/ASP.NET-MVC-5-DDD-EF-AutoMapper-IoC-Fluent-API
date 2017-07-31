using System;

namespace HHT.UI.ViewModels
{
    public class ContratadoViewModelMobile
    {
        public int ContratadoId { get; set; }
        public string Nome { get; set; }
        public string RG { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? ValidadeDocumento { get; set; }
        public bool Restricao { get; set; }
        public string Justificativa { get; set; }

        public int EmpresaId { get; set; }
        public string Empresa { get; set; }
    }
}