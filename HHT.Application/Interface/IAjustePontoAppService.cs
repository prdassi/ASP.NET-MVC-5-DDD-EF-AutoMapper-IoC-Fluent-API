using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public interface IAjustePontoAppService : IAppServiceBase<AjustePonto>
    {
        List<AjustePonto> ObterPontoDetalhado(int localId, int empresaId, int contratadoId, int ano, int mes, int dia);
    }
}
