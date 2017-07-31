using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;

namespace HHT.Application
{
    public class MappedEmpresaLocalAppService : AppServiceBase<MappedEmpresaLocal>, IMappedEmpresaLocalAppService
    {
        private readonly IMappedEmpresaLocalService _mappedEmpresalocalService;

        public MappedEmpresaLocalAppService(IMappedEmpresaLocalService mappedEmpresalocalService) : base (mappedEmpresalocalService)
        {
            _mappedEmpresalocalService = mappedEmpresalocalService;
        }
    }
}
