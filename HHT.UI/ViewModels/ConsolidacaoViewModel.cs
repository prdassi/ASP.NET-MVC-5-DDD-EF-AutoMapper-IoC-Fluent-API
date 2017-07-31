using System.Collections.Generic;

namespace HHT.UI.ViewModels
{
    public class ConsolidacaoViewModel
    {
        public ConsolidacaoViewModel()
        {
            this.Meses = new List<MesViewModel>();
        }

        public string Empresa { get; set; }
        public string Gestor { get; set; }
        public List<MesViewModel> Meses { get; set; }

        public class MesViewModel
        {
            public string Nome { get; set; }
            public int NumeroFuncionario { get; set; }
            public int HHT { get; set; }
        }
    }
}