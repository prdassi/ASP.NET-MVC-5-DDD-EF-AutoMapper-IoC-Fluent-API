using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;


namespace HHT.Domain.Services
{
    public class MappedEmpresaLocalService : ServiceBase<MappedEmpresaLocal>, IMappedEmpresaLocalService
    {
        private readonly IMappedEmpresaLocalRepository _mappedEmpresalocalRepository;

        public MappedEmpresaLocalService(IMappedEmpresaLocalRepository mappedEmpresalocalRepository) : base (mappedEmpresalocalRepository)
        {
            _mappedEmpresalocalRepository = mappedEmpresalocalRepository;
        }
    }
}
