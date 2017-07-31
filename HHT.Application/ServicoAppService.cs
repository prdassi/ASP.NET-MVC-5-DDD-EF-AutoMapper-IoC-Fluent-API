using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;

namespace HHT.Application.Interface
{
    public class ServicoAppService : AppServiceBase<Servico>, IServicoAppService
    {
        private readonly IServicoService _servicoService;

        public ServicoAppService(IServicoService servicoService) : base(servicoService)
        {
            _servicoService = servicoService;
        }

        public int[] ObterServicosSelecionados(Empresa empresa)
        {
            return _servicoService.ObterServicosSelecionados(empresa);
        }
    }
}
