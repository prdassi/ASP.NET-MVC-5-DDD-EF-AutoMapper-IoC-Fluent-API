using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface ITipoDocumentoRepository : IRepositoryBase<TipoDocumento>
    {
        IEnumerable<TipoDocumento> ObterTiposDocumentoContratado();
    }
}
