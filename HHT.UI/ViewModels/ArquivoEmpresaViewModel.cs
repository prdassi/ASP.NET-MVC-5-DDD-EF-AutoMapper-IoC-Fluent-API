using System;
using System.Collections.Generic;
using HHT.Domain.Entities;

namespace HHT.UI.ViewModels
{
    public class ArquivoEmpresaViewModel
    {
        public ArquivoEmpresaViewModel()
        {
            this.Empresas = new List<EmpresaViewModel>();
        }
        
        public int ArquivoEmpresaId { get; set; }
        public string Nome { get; set; }
        public string Tamanho { get; set; }
        public string Tipo { get; set; }
        public DateTime DataDocumento { get; set; }
        public DateTime DataCadastro { get; set; }

        public int DocumentoGeralId { get; set; }
        public virtual DocumentoGeralViewModel DocumentosGeral { get; set; }
        
        public virtual ICollection<EmpresaViewModel> Empresas { get; set; }
    }
}