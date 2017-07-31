using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HHT.UI.ViewModels
{
    public class UsuarioViewModel
    {
        public UsuarioViewModel()
        {
            this.MappedLocal = new List<MappedLocalViewModel>();
        }

        public int UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Perfil { get; set; }
        public bool Porteiro { get; set; }
        public DateTime DataCadastro { get; set; }

        public int[] LocaisSelecionados { get; set; }

        public int EmpresaId { get; set; }
        public virtual EmpresaViewModel Empresas { get; set; }
        
        public virtual ICollection<MappedLocalViewModel> MappedLocal { get; set; }
    }
    
}