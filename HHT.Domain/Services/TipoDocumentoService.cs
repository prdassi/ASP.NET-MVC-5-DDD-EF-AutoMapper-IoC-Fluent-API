using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class TipoDocumentoService : ServiceBase<TipoDocumento>, ITipoDocumentoService
    {
        private readonly ITipoDocumentoRepository _tipoDocumentoRepository;

        public TipoDocumentoService(ITipoDocumentoRepository tipoDocumentoRepository) : base (tipoDocumentoRepository)
        {
            _tipoDocumentoRepository = tipoDocumentoRepository;
        }

        public IEnumerable<TipoDocumento> ObterTiposDocumentoContratado()
        {
            return _tipoDocumentoRepository.ObterTiposDocumentoContratado();
        }
    }
}
