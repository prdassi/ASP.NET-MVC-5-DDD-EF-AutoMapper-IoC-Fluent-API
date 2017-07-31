using System;

namespace HHT.UI.ViewModels
{
    public class PontoViewModel
    {
        public int PontoId { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? HoraRegistro { get; set; }
        public string Registro { get; set; }
        public string ModoRegistro { get; set; }
        public string Justificativa { get; set; }

        public int ContratadoId { get; set; }
        public virtual ContratadoViewModel Contratado { get; set; }

        public int LocalId { get; set; }
        public virtual LocalViewModel Local { get; set; }
        
        public int UsuarioId { get; set; }
        public virtual UsuarioViewModel Usuario { get; set; }
    }
}