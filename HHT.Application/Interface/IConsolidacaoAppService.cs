using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public interface IConsolidacaoAppService : IAppServiceBase<Consolidacao>
    {
        IEnumerable<Consolidacao> ObterResultados(int localId, int empresaId, int ano, int mes);
    }
}
