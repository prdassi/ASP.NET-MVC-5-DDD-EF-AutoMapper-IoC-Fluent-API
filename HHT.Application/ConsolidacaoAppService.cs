using System;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Application
{
    public class ConsolidacaoAppService : AppServiceBase<Consolidacao>, IConsolidacaoAppService
    {
        private readonly IConsolidacaoService _consolidacaoService;

        public ConsolidacaoAppService(IConsolidacaoService consolidacaoService) : base(consolidacaoService)
        {
            _consolidacaoService = consolidacaoService;
        }

        public IEnumerable<Consolidacao> ObterResultados(int localId, int empresaId, int ano, int mes)
        {
            return _consolidacaoService.ObterResultados(localId, empresaId, ano, mes);
        }
    }
}
