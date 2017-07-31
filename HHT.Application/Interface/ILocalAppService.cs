using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public interface ILocalAppService : IAppServiceBase<Local>
    {
        int GetNull();
        int[] ObterLocaisEmpresaSeleciponados(Empresa empresa);
        IEnumerable<Local> ObterPorPerfil(string role, string userName);
        List<Local> ObterPorEmail(string email);
    }
}
