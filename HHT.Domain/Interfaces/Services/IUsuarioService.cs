using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Services
{
    public interface IUsuarioService : IServiceBase<Usuario>
    {
        void SalvarLocaisSelecionados(Usuario usuario);

        Usuario ObterLocaisPorId(Usuario usuario);

     //   Usuario ObterUsuarioPorLogin(string login, int usuarioId);

        Usuario Login(string usuario, string senha);

        void RemoveEspecifico(int usuarioId);

        Usuario ObterUsuarioLogado(string userName);

        int ObterIdPorEmail(string email);
    }
}
