using HHT.Domain.Entities;
using System.Web;

namespace HHT.Application.Interface
{
    public interface IArquivoEmpresaAppService : IAppServiceBase<ArquivoEmpresa>
    {
        void AdicionaDocumento(ArquivoEmpresa arrquivoEmpresa, int empresaId, HttpPostedFileBase upload);

        void EditarDocumento(ArquivoEmpresa arquivoEmpresa, HttpPostedFileBase upload);

        void ExcluirEmpresaCompleta(int arquivoEmpresaId, int empresaId);

        void ExcluirDocumento(int arquivoEmpresaId, int empresaId);

        ArquivoEmpresa ObterArquivoPorDocumento(int documentoId);
    }
}
