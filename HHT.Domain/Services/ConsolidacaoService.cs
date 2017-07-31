using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class ConsolidacaoService : ServiceBase<Consolidacao>, IConsolidacaoService
    {
        private readonly IConsolidacaoRepository _consolidacaoRepository;

        public ConsolidacaoService(IConsolidacaoRepository consolidacaoRepository) : base(consolidacaoRepository)
        {
            _consolidacaoRepository = consolidacaoRepository;
        }

        public IEnumerable<Consolidacao> ObterResultados(int localId, int empresaId, int ano, int mes)
        {
            return _consolidacaoRepository.ObterResultados(localId, empresaId, ano, mes);
        }
    }
}
