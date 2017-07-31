using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;

namespace HHT.Domain.Services
{
    public class AssociadoService : ServiceBase<Associado>, IAssociadoService
    {
        private readonly IAssociadoRepository _associadoRepository;

        public AssociadoService(IAssociadoRepository associadoRepository) : base (associadoRepository)
        {
            _associadoRepository = associadoRepository;
        }
    }
}
