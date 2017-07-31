using HHT.Domain.Entities;
using System;
using System.Collections.Generic;

namespace HHT.UI.ViewModels
{
    public class ContratadoViewModel
    {
        public ContratadoViewModel()
        {
            this.ArquivosContratado = new List<ArquivoContratadoViewModel>();
        }

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
        public virtual FuncaoViewModel Funcao { get; set; }

        public int EmpresaId { get; set; }
        public virtual EmpresaViewModel Empresa { get; set; }

        //Responsavel Avaliador é buscado dos usuarios
        public int UsuarioId { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }

        public virtual ICollection<ArquivoContratadoViewModel> ArquivosContratado { get; set; }
    }
}