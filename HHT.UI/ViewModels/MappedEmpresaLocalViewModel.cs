using System;
using System.Collections.Generic;

namespace HHT.UI.ViewModels
{
    public class MappedEmpresaLocalViewModel
    {
        public MappedEmpresaLocalViewModel()
        {
            this.Empresas = new List<EmpresaViewModel>();
        }

        public int MappedEmpresaLocalId { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }

        public int LocalId { get; set; }
        public virtual LocalViewModel Local { get; set; }

        public virtual ICollection<EmpresaViewModel> Empresas { get; set; }
    }
}