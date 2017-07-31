using System;
using System.Collections.Generic;

namespace HHT.UI.ViewModels
{
    public class MappedLocalViewModel
    {
        public MappedLocalViewModel()
        {
            this.Usuarios = new List<UsuarioViewModel>();
        }

        public int MappedLocalId { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }

        public int LocalId { get; set; }
        public virtual LocalViewModel Local { get; set; }

        public virtual ICollection<UsuarioViewModel> Usuarios { get; set; }
    }
}