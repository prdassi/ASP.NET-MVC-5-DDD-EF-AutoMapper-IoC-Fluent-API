using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Services
{
    public interface IConsolidacaoService : IServiceBase<Consolidacao>
    {
        IEnumerable<Consolidacao> ObterResultados(int localId, int empresaId, int ano, int mes);
    }
}
