using HHT.Domain.Entities;

namespace HHT.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        void SalvarLocaisSelecionados(Usuario usuario);

        Usuario ObterLocaisPorId(Usuario usuario);

        Usuario Login(string login, string senha);

        void RemoveEspecifico(int usuarioId);

        Usuario ObterUsuarioLogado(string userName);

        int ObterIdPorEmail(string email);
    }
}
