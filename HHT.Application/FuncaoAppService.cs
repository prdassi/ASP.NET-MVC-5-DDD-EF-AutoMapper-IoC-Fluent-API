using System;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;

namespace HHT.Application
{
    public class FuncaoAppService : AppServiceBase<Funcao>, IFuncaoAppService
    {
        private readonly IFuncaoService _funcaoService;

        public FuncaoAppService(IFuncaoService funcaoService) : base(funcaoService)
        {
            _funcaoService = funcaoService;
        }
    }
}
