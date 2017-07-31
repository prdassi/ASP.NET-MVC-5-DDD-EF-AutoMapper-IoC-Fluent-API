using System;
using System.Timers;

namespace HHT.Domain.Entities
{
    public class Ponto
    {
        public int PontoId { get; set; }
        public DateTime DataRegistro { get; set; }
        public DateTime? HoraRegistro { get; set; }
        public string Registro { get; set; }
        public string ModoRegistro { get; set; }
        public string Justificativa { get; set; }

        public int ContratadoId { get; set; }
        public virtual Contratado Contratado { get; set; }

        public int LocalId { get; set; }
        public virtual Local Local { get; set; }

        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}