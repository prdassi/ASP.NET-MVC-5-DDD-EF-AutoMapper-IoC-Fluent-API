using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface IHistoricoRepository : IRepositoryBase<Historico>
    {
        void Adicionar(Historico historico);

        IEnumerable<Historico> ObterPorId(int contratadoId);

        void RemoveById(int contratadoId);
    }
}
