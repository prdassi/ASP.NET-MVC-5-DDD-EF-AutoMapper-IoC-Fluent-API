
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class UsuarioService : ServiceBase<Usuario>, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository) : base (usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public void SalvarLocaisSelecionados(Usuario usuario)
        {
            _usuarioRepository.SalvarLocaisSelecionados(usuario);
        }

        public Usuario ObterLocaisPorId(Usuario usuario)
        {
            return _usuarioRepository.ObterLocaisPorId(usuario);
        }

        public Usuario Login(string login, string senha)
        {
            return _usuarioRepository.Login(login, senha);
        }

        public void RemoveEspecifico(int usuarioId)
        {
            _usuarioRepository.RemoveEspecifico(usuarioId);
        }

        public Usuario ObterUsuarioLogado(string userName)
        {
            return _usuarioRepository.ObterUsuarioLogado(userName);
        }

        public int ObterIdPorEmail(string email)
        {
            return _usuarioRepository.ObterIdPorEmail(email);
        }
    }
}
