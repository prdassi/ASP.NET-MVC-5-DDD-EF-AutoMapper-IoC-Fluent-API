using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HHT.Domain.Entities
{
    public class Empresa
    {
        public Empresa()
        {
            this.MappedServico = new List<MappedServico>();
            this.ArquivosEmpresa = new List<ArquivoEmpresa>();
            this.MappedEmpresaLocal = new List<MappedEmpresaLocal>();
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

        public virtual ICollection<ArquivoEmpresa> ArquivosEmpresa { get; set; }
        public virtual ICollection<MappedServico> MappedServico { get; set; }
        public virtual ICollection<MappedEmpresaLocal> MappedEmpresaLocal { get; set; }
    }
}
