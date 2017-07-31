using HHT.Domain.Entities;
using System;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface IServicoRepository : IRepositoryBase<Servico>
    {
        int[] ObterServicosSelecionados(Empresa empresa);
    }
}
