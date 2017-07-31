using System;
using System.Collections.Generic;

namespace HHT.Domain.Entities
{
    public class ArquivoEmpresa
    {
        public ArquivoEmpresa()
        {
            this.Empresas = new List<Empresa>();
        }

        public int ArquivoEmpresaId { get; set; }
        public string Nome { get; set; }
        public string Tamanho { get; set; }
        public string Tipo { get; set; }
        public DateTime DataDocumento { get; set; }
        public DateTime DataCadastro { get; set; }

        public int DocumentoGeralId { get; set; }
        public virtual DocumentoGeral DocumentosGeral { get; set; }    
        
        public virtual ICollection<Empresa> Empresas { get; set; }
    }
}