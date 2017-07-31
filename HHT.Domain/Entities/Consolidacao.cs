using System.Collections.Generic;

namespace HHT.Domain.Entities
{
    public class Consolidacao
    {

        public Consolidacao()
        {
            this.Meses = new List<Mes>();
        }

        public string Empresa { get; set; }
        public string Gestor { get; set; }
        public List<Mes> Meses { get; set; }

        public class Mes
        {
            public string Nome { get; set; }
            public int NumeroFuncionario { get; set; }
            public int HHT { get; set; }
        }
    }
}