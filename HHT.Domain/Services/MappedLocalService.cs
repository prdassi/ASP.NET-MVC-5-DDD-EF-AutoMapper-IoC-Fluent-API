using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;

namespace HHT.Domain.Services
{
    public class MappedLocalService : ServiceBase<MappedLocal>, IMappedLocalService
    {
        private readonly IMappedLocalRepository _mappedlocalRepository;

        public MappedLocalService(IMappedLocalRepository mappedlocalRepository) : base (mappedlocalRepository)
        {
            _mappedlocalRepository = mappedlocalRepository;
        }
    }
}
