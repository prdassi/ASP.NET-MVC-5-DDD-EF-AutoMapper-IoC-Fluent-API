using System;

namespace HHT.Domain.Entities
{
    public class Local
    {
        public int LocalId { get; set; }
        public string Sigla { get; set; }
        public string Nome { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}
