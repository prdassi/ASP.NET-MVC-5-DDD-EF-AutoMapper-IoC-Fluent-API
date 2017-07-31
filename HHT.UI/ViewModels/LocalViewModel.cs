using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HHT.UI.ViewModels
{
    public class LocalViewModel
    {
        public int LocalId { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}