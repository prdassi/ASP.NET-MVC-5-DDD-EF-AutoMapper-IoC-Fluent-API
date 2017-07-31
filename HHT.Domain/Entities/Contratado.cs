using System;
using System.Collections.Generic;

namespace HHT.Domain.Entities
{
    public class Contratado
    {
        public int ContratadoId { get; set; }
        public string Nome { get; set; }
        public string RG { get; set; }        
        public bool Ativo { get; set; }
        public DateTime DataAdmissao { get; set; }        
        public bool Restricao { get; set; }
        public DateTime? ValidadeDocumento { get; set; }
        public string Justificativa { get; set; }
        public DateTime DataCadastro { get; set; }

        public int FuncaoId { get; set; }
        public virtual Funcao Funcao { get; set; }

        public int EmpresaId { get; set; }
        public virtual Empresa Empresa { get; set; }

        //Responsavel Avaliador é buscado dos usuarios
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<ArquivoContratado> ArquivosContratado { get; set; }
    }
}
