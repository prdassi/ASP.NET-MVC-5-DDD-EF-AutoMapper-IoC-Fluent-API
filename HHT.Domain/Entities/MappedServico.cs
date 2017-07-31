using System;
using System.Collections.Generic;

namespace HHT.Domain.Entities
{
    public class MappedServico
    {
        public MappedServico()
        {
            this.Empresas = new List<Empresa>();
        }
        
        public int MappedServicoId { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }

        public int ServicoId { get; set; }
        public virtual Servico Servico { get; set; }

        public virtual ICollection<Empresa> Empresas { get; set; }
    }
}
