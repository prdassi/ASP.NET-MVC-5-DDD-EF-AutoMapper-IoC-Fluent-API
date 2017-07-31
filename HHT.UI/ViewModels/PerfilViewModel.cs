using System;

namespace HHT.UI.ViewModels
{
    public class PerfilViewModel
    {
        public int PerfilId { get; set; }
        public string Nome { get; set; }
        public bool IsSelected { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}