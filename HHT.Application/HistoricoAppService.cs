using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public class HistoricoAppService : AppServiceBase<Historico>, IHistoricoAppService
    {
        private readonly IHistoricoService _historicoService;

        public HistoricoAppService(IHistoricoService historicoService) : base(historicoService)
        {
            _historicoService = historicoService;
        }

        public void Adicionar(Historico historico)
        {
            _historicoService.Adicionar(historico);
        }

        public IEnumerable<Historico> ObterPorId(int contratadoId)
        {
            return _historicoService.ObterPorId(contratadoId);
        }

        public void RemoveById(int contratadoId)
        {
            _historicoService.RemoveById(contratadoId);
        }
    }
}
