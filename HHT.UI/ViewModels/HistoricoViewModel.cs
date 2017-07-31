using System;

namespace HHT.UI.ViewModels
{
    public class HistoricoViewModel
    {
        public int HistoricoId { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }

        public int ContratadoId { get; set; }
        public virtual ContratadoViewModel Contradado { get; set; }

        public int UsuarioId { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }
    }
}