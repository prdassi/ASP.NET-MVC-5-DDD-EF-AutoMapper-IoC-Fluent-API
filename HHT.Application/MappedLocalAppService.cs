using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;

namespace HHT.Application
{
    public class MappedLocalAppService : AppServiceBase<MappedLocal>, IMappedLocalAppService
    {
        private readonly IMappedLocalService _mappedlocalService;

        public MappedLocalAppService(IMappedLocalService mappedlocalService) : base (mappedlocalService)
        {
            _mappedlocalService = mappedlocalService;
        }
    }
}
