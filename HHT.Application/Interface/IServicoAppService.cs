using HHT.Domain.Entities;

namespace HHT.Application.Interface
{
    public interface IServicoAppService : IAppServiceBase<Servico>
    {
        int[] ObterServicosSelecionados(Empresa empresa);
    }
}
