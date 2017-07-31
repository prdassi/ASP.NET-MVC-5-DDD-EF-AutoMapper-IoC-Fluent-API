using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public class TipoDocumentoAppService : AppServiceBase<TipoDocumento>, ITipoDocumentoAppService
    {
        private readonly ITipoDocumentoService _tipoDocumentoService;

        public TipoDocumentoAppService(ITipoDocumentoService tipoDocumentoService) : base(tipoDocumentoService)
        {
            _tipoDocumentoService = tipoDocumentoService;
        }

        public IEnumerable<TipoDocumento> ObterTiposDocumentoContratado()
        {
            return _tipoDocumentoService.ObterTiposDocumentoContratado();
        }
    }
}
