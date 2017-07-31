using HHT.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HHT.UI.ViewModels
{
    public class EmpresaViewModel
    {
        public EmpresaViewModel()
        {
            this.ArquivosEmpresa = new List<ArquivoEmpresaViewModel>();
            this.MappedServico = new List<MappedServicoViewModel>();
            this.MappedEmpresaLocal = new List<MappedEmpresaLocalViewModel>();
        }

        public int EmpresaId { get; set; }
        public string Nome { get; set; }
        public string CNPJ { get; set; }
        public string NomeGestorAES { get; set; }
        public string EmailGestorAES { get; set; }
        public string NomeResponsavelEmpresa { get; set; }
        public string EmailResponsavelEmpresa { get; set; }
        public string NomeMonitorAES { get; set; }
        public string EmailMonitorAES { get; set; }
        public bool EmpresaAtiva { get; set; }
        public DateTime? ValidadeDocumento { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Contrato { get; set; }
        public string VerificarArquivo { get; set; }

        public int[] ServicosSelecionados { get; set; }
        public int[] LocaisSelecionados { get; set; }

        //public int LocalId { get; set; }
        public virtual LocalViewModel Local { get; set; }

        public List<Local> TodosLocais { get; set; }

        public virtual ICollection<ArquivoEmpresaViewModel> ArquivosEmpresa { get; set; }
        public virtual ICollection<MappedEmpresaLocalViewModel> MappedEmpresaLocal { get; set; }
        public virtual ICollection<MappedServicoViewModel> MappedServico { get; set; }
    }
}