using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Web;

namespace HHT.Domain.Services
{
    public class ArquivoEmpresaService : ServiceBase<ArquivoEmpresa>, IArquivoEmpresaService
    {
        private readonly IArquivoEmpresaRepository _arquivoEmpresaRepository;

        public ArquivoEmpresaService(IArquivoEmpresaRepository arquivoEmpresaRepository) : base(arquivoEmpresaRepository)
        {
            _arquivoEmpresaRepository = arquivoEmpresaRepository;
        }

        public void AdicionaDocumento(ArquivoEmpresa arquivoEmpresa, int empresaId, HttpPostedFileBase upload)
        {
            _arquivoEmpresaRepository.AdicionaDocumento(arquivoEmpresa, empresaId, upload);
        }

        public void EditarDocumento(ArquivoEmpresa arquivoEmpresa, HttpPostedFileBase upload)
        {
            _arquivoEmpresaRepository.EditarDocumento(arquivoEmpresa, upload);
        }

        public void ExcluirEmpresaCompleta(int arquivoEmpresaId, int empresaId)
        {
            _arquivoEmpresaRepository.ExcluirEmpresaCompleta(arquivoEmpresaId, empresaId);
        }

        public void ExcluirDocumento(int arquivoEmpresaId, int empresaId)
        {
            _arquivoEmpresaRepository.ExcluirDocumento(arquivoEmpresaId, empresaId);
        }

        public ArquivoEmpresa ObterArquivoPorDocumento(int documentoId)
        {
            return _arquivoEmpresaRepository.ObterArquivoPorDocumento(documentoId);
        }
    }
}
