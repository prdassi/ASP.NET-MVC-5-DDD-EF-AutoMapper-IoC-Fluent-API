using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Services
{
    public interface IDocumentoGeralService : IServiceBase<DocumentoGeral>
    {
        IEnumerable<DocumentoGeral> ObterDocumentosContratado();

        IEnumerable<DocumentoGeral> ObterDocumentosEmpresa();

        IEnumerable<DocumentoGeral> ObterDocumentosRestantes(int empresaId);
        IEnumerable<DocumentoGeral> ObterDocumentosRestantesIncluido(int empresaId, int arquivoEmpresaId);
    }
}
