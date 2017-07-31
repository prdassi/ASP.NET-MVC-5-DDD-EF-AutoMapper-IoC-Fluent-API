using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface IConsolidacaoRepository : IRepositoryBase<Consolidacao>
    {
        IEnumerable<Consolidacao> ObterResultados(int localId, int empresaId, int ano, int mes);
    }
}
