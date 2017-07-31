using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;

namespace HHT.Domain.Services
{
    public class MappedServicoService : ServiceBase<MappedServico>, IMappedServicoService
    {
        private readonly IMappedServicoRepository _mappedServicoRepository;

        public MappedServicoService(IMappedServicoRepository mappedServicoRepository) : base (mappedServicoRepository)
        {
            _mappedServicoRepository = mappedServicoRepository;
        }
    }
}
