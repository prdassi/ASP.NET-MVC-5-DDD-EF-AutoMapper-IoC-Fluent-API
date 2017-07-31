using System;
using System.Collections.Generic;

namespace HHT.Domain.Entities
{
    public class MappedEmpresaLocal
    {
        public MappedEmpresaLocal()
        {
            this.Empresas = new List<Empresa>();
        }

        public int MappedEmpresaLocalId { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }

        public int LocalId { get; set; }
        public virtual Local Local { get; set; }

        public virtual ICollection<Empresa> Empresas { get; set; }
    }
}
