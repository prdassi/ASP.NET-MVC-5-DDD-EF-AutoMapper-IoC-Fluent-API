using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Web;

namespace HHT.Application
{
    public class ArquivoContratadoAppService : AppServiceBase<ArquivoContratado>, IArquivoContratadoAppService
    {
        private readonly IArquivoContratadoService _arquivoContratadoService;

        public ArquivoContratadoAppService(IArquivoContratadoService arquivoContratadoService) : base(arquivoContratadoService)
        {
            _arquivoContratadoService = arquivoContratadoService;
        }

        public void AdicionaDocumento(ArquivoContratado arquivoContratado, int contratadoId, HttpPostedFileBase upload)
        {
            _arquivoContratadoService.AdicionaDocumento(arquivoContratado, contratadoId, upload);
        }

        public void EditarDocumento(ArquivoContratado arquivoContratado, HttpPostedFileBase upload, int contratadoId)
        {
            _arquivoContratadoService.EditarDocumento(arquivoContratado, upload, contratadoId);
        }

        public void ExcluirContratadoCompleto(int arquivoContratadoId, int contratadoId)
        {
            _arquivoContratadoService.ExcluirContratadoCompleto(arquivoContratadoId, contratadoId);
        }

        public void ExcluirDocumento(int arquivoContratadoId, int contratadoId)
        {
            _arquivoContratadoService.ExcluirDocumento(arquivoContratadoId, contratadoId);
        }

        public ArquivoContratado ObterArquivoPorDocumento(int documentoId)
        {
            return _arquivoContratadoService.ObterArquivoPorDocumento(documentoId);
        }

        public void AjustarVencimentoCurso(ArquivoContratado arquivoContratado, int contratadoId)
        {
            _arquivoContratadoService.AjustarVencimentoCurso(arquivoContratado, contratadoId);
        }
    }
}
