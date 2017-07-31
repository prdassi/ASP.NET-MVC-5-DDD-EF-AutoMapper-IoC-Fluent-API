using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface ILocalRepository : IRepositoryBase<Local>
    {
        int GetNull();
        int[] ObterLocaisEmpresaSeleciponados(Empresa empresa);
        IEnumerable<Local> ObterPorPerfil(string role, string userName);
        List<Local> ObterPorEmail(string email);
    }
}
