using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;

namespace HHT.Application
{
    public class AssociadoAppService : AppServiceBase<Associado>, IAssociadoAppService
    {
        private readonly IAssociadoService _associadoService;

        public AssociadoAppService(IAssociadoService associadoService) : base (associadoService)
        {
            _associadoService = associadoService;
        }
    }
}
