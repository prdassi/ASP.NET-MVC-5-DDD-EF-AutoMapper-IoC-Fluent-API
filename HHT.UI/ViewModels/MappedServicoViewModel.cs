using System;
using System.Collections.Generic;

namespace HHT.UI.ViewModels
{
    public class MappedServicoViewModel
    {
        public MappedServicoViewModel()
        {
            this.Empresas = new List<EmpresaViewModel>();
        }

        public int MappedServicoId { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }

        public int ServicoId { get; set; }
        public virtual ServicoViewModel Servico { get; set; }

        public virtual ICollection<EmpresaViewModel> Empresas { get; set; }
    }
}