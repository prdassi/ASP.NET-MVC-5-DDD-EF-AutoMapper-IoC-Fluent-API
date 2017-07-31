using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public interface IDocumentoGeralAppService : IAppServiceBase<DocumentoGeral>
    {
        IEnumerable<DocumentoGeral> ObterDocumentosContratado();
        IEnumerable<DocumentoGeral> ObterDocumentosEmpresa();
        IEnumerable<DocumentoGeral> ObterDocumentosRestantes(int empresaId);
        IEnumerable<DocumentoGeral> ObterDocumentosRestantesIncluido(int empresaId, int arquivoEmpresaId);
    }
}
