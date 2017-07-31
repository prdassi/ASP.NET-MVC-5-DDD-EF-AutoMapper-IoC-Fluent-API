using HHT.Domain.Entities;
using System;

namespace HHT.Domain.Interfaces.Services
{
    public interface IServicoService : IServiceBase<Servico>
    {
        int[] ObterServicosSelecionados(Empresa empresa);
    }
}
