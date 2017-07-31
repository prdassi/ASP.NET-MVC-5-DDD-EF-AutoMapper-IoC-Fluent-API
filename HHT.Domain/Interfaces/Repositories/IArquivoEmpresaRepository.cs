using HHT.Domain.Entities;
using System.Web;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface IArquivoEmpresaRepository : IRepositoryBase<ArquivoEmpresa>
    {
        void AdicionaDocumento(ArquivoEmpresa arquivoEmpresa, int empresaId, HttpPostedFileBase upload);

        void EditarDocumento(ArquivoEmpresa arquivoEmpresa, HttpPostedFileBase upload);

        void ExcluirEmpresaCompleta(int arquivoEmpresaId, int empresaId);

        void ExcluirDocumento(int arquivoEmpresaId, int empresaId);

        ArquivoEmpresa ObterArquivoPorDocumento(int documentoId);
    }
}
