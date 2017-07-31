using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;

namespace HHT.Domain.Services
{
    public class ServicoService : ServiceBase<Servico>, IServicoService
    {
        private readonly IServicoRepository _servicoRepository;

        public ServicoService(IServicoRepository servicoRepository) : base (servicoRepository)
        {
            _servicoRepository = servicoRepository;
        }

        public int[] ObterServicosSelecionados(Empresa empresa)
        {
            return _servicoRepository.ObterServicosSelecionados(empresa);
        }
    }
}
