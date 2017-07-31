using System;
using System.Collections.Generic;

namespace HHT.Domain.Entities
{
    public class Usuario
    {
        public Usuario()
        {
            this.MappedLocal = new List<MappedLocal>();
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
        public virtual Empresa Empresas { get; set; }
        
        public virtual ICollection<MappedLocal> MappedLocal { get; set; }            
    }
}
