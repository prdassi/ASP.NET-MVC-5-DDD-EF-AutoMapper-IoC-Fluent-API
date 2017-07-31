using System;

namespace HHT.Domain.Entities
{
    public class Historico
    {
        public int HistoricoId { get; set; }
        public DateTime Data { get; set; }
        public string Descricao { get; set; }

        public int ContratadoId { get; set; }
        public virtual Contratado Contradado { get; set; }

        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}
