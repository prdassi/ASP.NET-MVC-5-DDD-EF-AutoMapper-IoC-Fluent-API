using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Services
{
    public interface ITipoDocumentoService : IServiceBase<TipoDocumento>
    {
        IEnumerable<TipoDocumento> ObterTiposDocumentoContratado();
    }
}
