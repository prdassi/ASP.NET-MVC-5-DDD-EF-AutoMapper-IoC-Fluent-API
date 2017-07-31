using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface IContratadoRepository : IRepositoryBase<Contratado>
    {
        Contratado ObterDocumentos(int contratadoId);
        Contratado ObterPorRG(int localId, string rg);
        IEnumerable<Contratado> ObterPorNome(int localId, string nome);
        IEnumerable<Contratado> ObterPorLocal(int localId);
        IEnumerable<Contratado> ObterPorEmpresa(int empresaId);
        Identificacao Identificacao(int contratadoId);
        IEnumerable<Contratado> ObterPorPerfil(string role, string userName);
    }
}
