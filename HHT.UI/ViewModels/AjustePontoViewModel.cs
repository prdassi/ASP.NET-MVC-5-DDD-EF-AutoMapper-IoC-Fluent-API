using System;
using System.Collections.Generic;

namespace HHT.UI.ViewModels
{
    public class AjustePontoViewModel
    {
        public int ContratadoId { get; set; }
        public string Nome { get; set; }
        public string RG { get; set; }
        public int Ano { get; set; }
        public string Mes { get; set; }
        public int NumeroDia { get; set; }
        public TimeSpan? TotalHHT { get; set; }
        public string HHT { get; set; }
    }
}