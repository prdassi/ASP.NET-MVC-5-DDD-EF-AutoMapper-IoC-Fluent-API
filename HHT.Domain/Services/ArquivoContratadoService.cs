using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Web;

namespace HHT.Domain.Services
{
    public class ArquivoContratadoService : ServiceBase<ArquivoContratado>, IArquivoContratadoService
    {
        private readonly IArquivoContratadoRepository _arquivoContratadoRepository;

        public ArquivoContratadoService(IArquivoContratadoRepository arquivoContratadoRepository) : base(arquivoContratadoRepository)
        {
            _arquivoContratadoRepository = arquivoContratadoRepository;
        }

        public void AdicionaDocumento(ArquivoContratado arquivoContratado, int empresaId, HttpPostedFileBase upload)
        {
            _arquivoContratadoRepository.AdicionaDocumento(arquivoContratado, empresaId, upload);
        }

        public void EditarDocumento(ArquivoContratado arquivoContratado, HttpPostedFileBase upload, int contratadoId)
        {
            _arquivoContratadoRepository.EditarDocumento(arquivoContratado, upload, contratadoId);
        }

        public void ExcluirContratadoCompleto(int arquivoContratadoId, int contratadoId)
        {
            _arquivoContratadoRepository.ExcluirContratadoCompleto(arquivoContratadoId, contratadoId);
        }

        public void ExcluirDocumento(int arquivoEmpresaId, int empresaId)
        {
            _arquivoContratadoRepository.ExcluirDocumento(arquivoEmpresaId, empresaId);
        }

        public ArquivoContratado ObterArquivoPorDocumento(int documentoId)
        {
            return _arquivoContratadoRepository.ObterArquivoPorDocumento(documentoId);
        }

        public void AjustarVencimentoCurso(ArquivoContratado arquivoContratado, int contratadoId)
        {
            _arquivoContratadoRepository.AjustarVencimentoCurso(arquivoContratado, contratadoId);
        }
    }
}
