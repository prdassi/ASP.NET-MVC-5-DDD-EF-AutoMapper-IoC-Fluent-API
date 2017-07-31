using HHT.Domain.Entities;
using System.Web;

namespace HHT.Application.Interface
{
    public interface IArquivoContratadoAppService : IAppServiceBase<ArquivoContratado>
    {
        void AdicionaDocumento(ArquivoContratado arrquivoContratado, int contratadoId, HttpPostedFileBase upload);

        void EditarDocumento(ArquivoContratado arrquivoContratado, HttpPostedFileBase upload, int contratadoId);

        void ExcluirContratadoCompleto(int arquivoContratadoId, int contratadoId);

        void ExcluirDocumento(int arquivoContratadoId, int contratadoId);

        ArquivoContratado ObterArquivoPorDocumento(int documentoId);

        void AjustarVencimentoCurso(ArquivoContratado arquivoContratado, int contratadoId);
    }
}
