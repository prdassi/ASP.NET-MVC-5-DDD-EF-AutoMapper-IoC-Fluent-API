using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class DocumentoGeralService : ServiceBase<DocumentoGeral>, IDocumentoGeralService
    {
        private readonly IDocumentoGeralRepository _documentoGeralRepository;

        public DocumentoGeralService(IDocumentoGeralRepository documentoGeralRepository) : base (documentoGeralRepository)
        {
            _documentoGeralRepository = documentoGeralRepository;
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosContratado()
        {
            return _documentoGeralRepository.ObterDocumentosContratado();
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosEmpresa()
        {
            return _documentoGeralRepository.ObterDocumentosEmpresa();
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosRestantes(int empresaId)
        {
            return _documentoGeralRepository.ObterDocumentosRestantes(empresaId);
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosRestantesIncluido(int empresaId, int arquivoEmpresaId)
        {
            return _documentoGeralRepository.ObterDocumentosRestantesIncluido(empresaId, arquivoEmpresaId);
        }
    }
}
