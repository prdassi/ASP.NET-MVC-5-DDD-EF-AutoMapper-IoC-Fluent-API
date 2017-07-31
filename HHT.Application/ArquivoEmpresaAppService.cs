using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Web;

namespace HHT.Application
{
    public class ArquivoEmpresaAppService : AppServiceBase<ArquivoEmpresa>, IArquivoEmpresaAppService
    {
        private readonly IArquivoEmpresaService _arquivoEmpresaService;

        public ArquivoEmpresaAppService(IArquivoEmpresaService arquivoEmpresaService) : base(arquivoEmpresaService)
        {
            _arquivoEmpresaService = arquivoEmpresaService;
        }

        public void AdicionaDocumento(ArquivoEmpresa arquivoEmpresa, int empresaId, HttpPostedFileBase upload)
        {
            _arquivoEmpresaService.AdicionaDocumento(arquivoEmpresa, empresaId, upload);
        }

        public void EditarDocumento(ArquivoEmpresa arquivoEmpresa, HttpPostedFileBase upload)
        {
            _arquivoEmpresaService.EditarDocumento(arquivoEmpresa, upload);
        }

        public void ExcluirEmpresaCompleta(int arquivoEmpresaId, int empresaId)
        {
            _arquivoEmpresaService.ExcluirEmpresaCompleta(arquivoEmpresaId, empresaId);
        }

        public void ExcluirDocumento(int arquivoEmpresaId, int empresaId)
        {
            _arquivoEmpresaService.ExcluirDocumento(arquivoEmpresaId, empresaId);
        }

        public ArquivoEmpresa ObterArquivoPorDocumento(int documentoId)
        {
            return _arquivoEmpresaService.ObterArquivoPorDocumento(documentoId);
        }
    }
}
