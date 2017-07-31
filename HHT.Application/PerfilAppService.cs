using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;

namespace HHT.Application
{
    public class PerfilAppService : AppServiceBase<Perfil>, IPerfilAppService
    {
        private readonly IPerfilService _perfilService;

        public PerfilAppService(IPerfilService perfilService) : base (perfilService)
        {
            _perfilService = perfilService;
        }
    }
}
