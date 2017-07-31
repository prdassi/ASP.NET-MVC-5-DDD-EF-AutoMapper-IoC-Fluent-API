using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;

namespace HHT.Application
{
    public class MappedServicoAppService : AppServiceBase<MappedServico>, IMappedServicoAppService
    {
        private readonly IMappedServicoService _mappedServicoService;

        public MappedServicoAppService(IMappedServicoService mappedServicoService) : base (mappedServicoService)
        {
            _mappedServicoService = mappedServicoService;
        }
    }
}
