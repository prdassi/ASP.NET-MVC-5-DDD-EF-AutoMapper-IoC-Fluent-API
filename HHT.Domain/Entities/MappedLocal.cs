using System;
using System.Collections.Generic;

namespace HHT.Domain.Entities
{
    public class MappedLocal
    {
        public MappedLocal()
        {
            this.Usuarios = new List<Usuario>();
        }

        public int MappedLocalId { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }

        public int LocalId { get; set; }
        public virtual Local Local { get; set; }

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
