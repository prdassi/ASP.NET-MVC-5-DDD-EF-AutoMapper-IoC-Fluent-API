using HHT.Domain.Entities;
using System.Web;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface IArquivoContratadoRepository : IRepositoryBase<ArquivoContratado>
    {
        void AdicionaDocumento(ArquivoContratado arquivoContratado, int contratadoId, HttpPostedFileBase upload);

        void EditarDocumento(ArquivoContratado arquivoContratado, HttpPostedFileBase upload, int contratadoId);

        void ExcluirContratadoCompleto(int arquivoContratadoId, int contratadoId);

        void ExcluirDocumento(int arquivoContratadoId, int contratadoId);

        ArquivoContratado ObterArquivoPorDocumento(int documentoId);
        void AjustarVencimentoCurso(ArquivoContratado arquivoContratado, int contratadoId);
    }
}
