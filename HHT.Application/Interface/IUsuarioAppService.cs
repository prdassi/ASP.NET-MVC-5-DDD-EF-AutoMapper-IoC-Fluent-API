using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Application.Interface
{
    public interface IUsuarioAppService : IAppServiceBase<Usuario>
    {
        void SalvarLocaisSelecionados(Usuario usuario);

        Usuario ObterLocaisPorId(Usuario usuario);

        Usuario Login(string login, string senha);

    //    Usuario ObterUsuarioPorLogin(string login, int usuarioId);

        void RemoveEspecifico(int usuarioId);

        Usuario ObterUsuarioLogado(string userName);

        int ObterIdPorEmail(string email);
    }
}
