using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public interface ITipoDocumentoAppService : IAppServiceBase<TipoDocumento>
    {
        IEnumerable<TipoDocumento> ObterTiposDocumentoContratado();
    }
}
