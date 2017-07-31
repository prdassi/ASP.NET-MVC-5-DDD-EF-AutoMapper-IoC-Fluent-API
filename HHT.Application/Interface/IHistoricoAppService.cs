using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public interface IHistoricoAppService : IAppServiceBase<Historico>
    {
        void Adicionar(Historico historico);

        IEnumerable<Historico> ObterPorId(int contratadoId);

        void RemoveById(int contratadoId);
    }
}
