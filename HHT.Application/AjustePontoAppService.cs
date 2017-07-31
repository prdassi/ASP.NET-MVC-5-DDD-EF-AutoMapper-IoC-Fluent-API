using System;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Application
{
    public class AjustePontoAppService : AppServiceBase<AjustePonto>, IAjustePontoAppService
    {
        private readonly IAjustePontoService _ajustePontoService;

        public AjustePontoAppService(IAjustePontoService ajustePontoService) : base(ajustePontoService)
        {
            _ajustePontoService = ajustePontoService;
        }

        public List<AjustePonto> ObterPontoDetalhado(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        {
            return _ajustePontoService.ObterPontoDetalhado(localId, empresaId, contratadoId, ano, mes, dia);
        }
    }
}
