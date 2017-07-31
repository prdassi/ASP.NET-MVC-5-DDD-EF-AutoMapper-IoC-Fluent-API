using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class ContratadoService : ServiceBase<Contratado>, IContratadoService
    {
        private readonly IContratadoRepository _contratadoRepository;

        public ContratadoService(IContratadoRepository contratadoRepository) : base (contratadoRepository)
        {
            _contratadoRepository = contratadoRepository;
        }

        public Contratado ObterDocumentos(int contratadoId)
        {
            return _contratadoRepository.ObterDocumentos(contratadoId);
        }

        public Contratado ObterPorRG(int localId, string rg)
        {
            return _contratadoRepository.ObterPorRG(localId, rg);
        }

        public IEnumerable<Contratado> ObterPorNome(int localId, string nome)
        {
            return _contratadoRepository.ObterPorNome(localId, nome);
        }
        public IEnumerable<Contratado> ObterPorLocal(int localId)
        {
            return _contratadoRepository.ObterPorLocal(localId);
        }
        public IEnumerable<Contratado> ObterPorEmpresa(int empresaId)
        {
            return _contratadoRepository.ObterPorEmpresa(empresaId);
        }

        public Identificacao Identificacao(int contratadoId)
        {
            return _contratadoRepository.Identificacao(contratadoId);
        }

        public IEnumerable<Contratado> ObterPorPerfil(string role, string userName)
        {
            return _contratadoRepository.ObterPorPerfil(role, userName);
        }
    }
}
