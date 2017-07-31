using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Services
{
    public interface IHistoricoService : IServiceBase<Historico>
    {
        void Adicionar(Historico historico);

        IEnumerable<Historico> ObterPorId(int contratadoId);

        void RemoveById(int contratadoId);
    }
}
