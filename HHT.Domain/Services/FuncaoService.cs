using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;

namespace HHT.Domain.Services
{
    public class FuncaoService : ServiceBase<Funcao>, IFuncaoService
    {
        private readonly IFuncaoRepository _funcaoRepository;

        public FuncaoService(IFuncaoRepository funcaoRepository) : base(funcaoRepository)
        {
            _funcaoRepository = funcaoRepository;
        }
    }
}
