using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class HistoricoService : ServiceBase<Historico>, IHistoricoService
    {
        private readonly IHistoricoRepository _historicoRepository;

        public HistoricoService(IHistoricoRepository historicoRepository) : base (historicoRepository)
        {
            _historicoRepository = historicoRepository;
        }

        public void Adicionar(Historico historico)
        {
            _historicoRepository.Adicionar(historico);
        }

        public IEnumerable<Historico> ObterPorId(int contratadoId)
        {
            return _historicoRepository.ObterPorId(contratadoId);
        }

        public void RemoveById(int contratadoId)
        {
            _historicoRepository.RemoveById(contratadoId);
        }
    }
}
