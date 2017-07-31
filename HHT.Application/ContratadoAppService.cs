using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public class ContratadoAppService : AppServiceBase<Contratado>, IContratadoAppService
    {
        private readonly IContratadoService _contratadoService;

        public ContratadoAppService(IContratadoService contratadoService) : base(contratadoService)
        {
            _contratadoService = contratadoService;
        }

        public Contratado ObterDocumentos(int contratadoId)
        {
            return _contratadoService.ObterDocumentos(contratadoId);
        }

        public Contratado ObterPorRG(int localId, string rg)
        {
            return _contratadoService.ObterPorRG(localId, rg);
        }

        public IEnumerable<Contratado> ObterPorNome(int localId, string nome)
        {
            return _contratadoService.ObterPorNome(localId, nome);
        }

        public IEnumerable<Contratado> ObterPorLocal(int localId)
        {
            return _contratadoService.ObterPorLocal(localId);
        }

        public IEnumerable<Contratado> ObterPorEmpresa(int empresaId)
        {
            return _contratadoService.ObterPorEmpresa(empresaId);
        }
        public Identificacao Identificacao(int contratadoId)
        {
            return _contratadoService.Identificacao(contratadoId);
        }

        public IEnumerable<Contratado> ObterPorPerfil(string role, string userName)
        {
            return _contratadoService.ObterPorPerfil(role, userName);
        }
    }
}
