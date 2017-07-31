using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Application
{
    public class UsuarioAppService : AppServiceBase<Usuario>, IUsuarioAppService
    {
        private readonly IUsuarioService _usuarioService;

        public UsuarioAppService(IUsuarioService usuarioService) : base(usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public void SalvarLocaisSelecionados(Usuario usuario)
        {
            _usuarioService.SalvarLocaisSelecionados(usuario);
        }

        public Usuario ObterLocaisPorId(Usuario usuario)
        {
            return _usuarioService.ObterLocaisPorId(usuario);
        }

        public Usuario Login(string login, string senha)
        {
            return _usuarioService.Login(login, senha);
        }

        //public Usuario ObterUsuarioPorLogin(string login, int usuarioId);
        //{
        //    return _usuarioService.ObterUsuarioPorLogin(login,usuarioId);
        //}

        public void RemoveEspecifico(int usuarioId)
        {
            _usuarioService.RemoveEspecifico(usuarioId);
        }

        public Usuario ObterUsuarioLogado(string userName)
        {
            return _usuarioService.ObterUsuarioLogado(userName);
        }

        public int ObterIdPorEmail(string email)
        {
            return _usuarioService.ObterIdPorEmail(email);
        }
    }
}
