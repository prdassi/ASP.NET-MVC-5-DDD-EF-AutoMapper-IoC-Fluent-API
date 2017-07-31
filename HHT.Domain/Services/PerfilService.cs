using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;

namespace HHT.Domain.Services
{
    public class PerfilService : ServiceBase<Perfil>, IPerfilService
    {
        private readonly IPerfilRepository _perfilRepository;

        public PerfilService(IPerfilRepository perfilRepository) : base (perfilRepository)
        {
            _perfilRepository = perfilRepository;
        }
    }
}
