using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface IAjustePontoRepository : IRepositoryBase<AjustePonto>
    {
        List<AjustePonto> ObterPontoDetalhado(int localId, int empresaId, int contratadoId, int ano, int mes, int dia);
    }
}
