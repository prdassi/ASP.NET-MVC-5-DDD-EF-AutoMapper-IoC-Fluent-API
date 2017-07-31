using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public class DocumentoGeralAppService : AppServiceBase<DocumentoGeral>, IDocumentoGeralAppService
    {
        private readonly IDocumentoGeralService _documentoGeralService;

        public DocumentoGeralAppService(IDocumentoGeralService documentoGeralService) : base(documentoGeralService)
        {
            _documentoGeralService = documentoGeralService;
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosContratado()
        {
            return _documentoGeralService.ObterDocumentosContratado();
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosEmpresa()
        {
            return _documentoGeralService.ObterDocumentosEmpresa();
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosRestantes(int empresaId)
        {
            return _documentoGeralService.ObterDocumentosRestantes(empresaId);
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosRestantesIncluido(int empresaId, int arquivoEmpresaId)
        {
            return _documentoGeralService.ObterDocumentosRestantesIncluido(empresaId, arquivoEmpresaId);
        }
    }
}
