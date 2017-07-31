using AutoMapper;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.UI.ViewModels;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using HHT.UI.Autenticacao;
using HHT.Infra.CrossCutting.Helper;

namespace HHT.UI.Controllers
{
    [Authorize]
    public class UsuarioController : Controller
    {
        private readonly ILocalAppService _localApp;
        private readonly IUsuarioAppService _usuarioApp;
        private readonly IEmpresaAppService _empresaApp;
        private readonly IServicoAppService _servicoApp;
        private readonly IPerfilAppService _perfilApp;
        private readonly IMappedLocalAppService _mappedLocalApp;

        public UsuarioController(ILocalAppService localApp, IUsuarioAppService usuarioApp, IEmpresaAppService empresaApp,
                                    IServicoAppService servicoApp, IMappedLocalAppService mappedLocalApp, IPerfilAppService perfilApp)
        {
            _localApp = localApp;
            _usuarioApp = usuarioApp;
            _empresaApp = empresaApp;
            _servicoApp = servicoApp;
            _mappedLocalApp = mappedLocalApp;
            _perfilApp = perfilApp;
        }

        // Definindo a instancia UserManager presente no request.
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // Definindo a instancia UserManager presente no request.
        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }


        // Definindo a instancia SignInManager presente no request.
        private ApplicationSignInManager _signInManager;
        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        // GET: Usuario
        public ActionResult Index()
        {
            ViewBag.Acesso = new SelectList(_perfilApp.GetAll(), "PerfilId", "Nome");

            var usuarioViewModel = Mapper.Map<IEnumerable<Usuario>, IEnumerable<UsuarioViewModel>>(_usuarioApp.GetAll());

            return View(usuarioViewModel);
        }

        public ActionResult Cadastrar()
        {
            var locais = _localApp.GetAll().ToList();
            if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
            {
                locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)
            }

            ViewBag.Locais = new SelectList(locais, "LocalId", "Nome");
            ViewBag.Empresas = new SelectList(_empresaApp.GetAll().OrderBy(x => x.Nome), "EmpresaId", "Nome");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Cadastrar(UsuarioViewModel novoUsuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!novoUsuario.Porteiro)
                    {
                        List<int> list = new List<int>();

                        foreach (var localId in _localApp.GetAll().Select(l => l.LocalId).ToList())
                        {
                            list.Add(localId);
                        }

                        list.RemoveAt(0);
                        novoUsuario.LocaisSelecionados = list.Cast<int>().ToArray();
                    }

                    foreach (var idSelecionado in novoUsuario.LocaisSelecionados)
                    {
                        var local = _localApp.GetById(idSelecionado);
                        var localViewModel = Mapper.Map<Local, LocalViewModel>(local);

                        var mappedLocalViewModel = new MappedLocalViewModel
                        {
                            MappedLocalId = localViewModel.LocalId,
                            DataCadastro = localViewModel.DataCadastro,
                            Nome = localViewModel.Nome,
                            Sigla = localViewModel.Sigla,
                            LocalId = localViewModel.LocalId
                        };
                        novoUsuario.MappedLocal.Add(mappedLocalViewModel);
                    }

                    var usuarioDomain = Mapper.Map<UsuarioViewModel, Usuario>(novoUsuario);
                    usuarioDomain.Senha = Segurancao.Encrypt(usuarioDomain.Senha);
                    _usuarioApp.Add(usuarioDomain);
                    //Cria Usuário para Autenticação
                    var user = new ApplicationUser { UserName = novoUsuario.Email, Email = novoUsuario.Email };
                    var result = await UserManager.CreateAsync(user, novoUsuario.Senha);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                throw;
            }
        }

        public ActionResult Editar(int usuarioId)
        {
            try
            {
                var usuario = _usuarioApp.GetById(usuarioId);
                var usuarioViewModel = Mapper.Map<Usuario, UsuarioViewModel>(usuario);

                int[] myIndices = new int[usuarioViewModel.MappedLocal.Count()];

                List<int> list = new List<int>();

                foreach (var selecao in usuarioViewModel.MappedLocal)
                {
                    list.Add(selecao.Local.LocalId);

                }
                myIndices = list.Cast<int>().ToArray();

                var locais = _localApp.GetAll().ToList();
                if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
                {
                    locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)
                }

                ViewBag.Empresas = new SelectList(_empresaApp.GetAll(), "EmpresaId", "Nome");
                ViewBag.Locais = new MultiSelectList(locais, "LocalId", "Nome", myIndices);

                return View(usuarioViewModel);
            }
            catch (System.Exception)
            {

                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(UsuarioViewModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!usuario.Porteiro)
                    {
                        List<int> list = new List<int>();

                        foreach (var localId in _localApp.GetAll().Select(l => l.LocalId).ToList())
                        {
                            list.Add(localId);
                        }

                        list.RemoveAt(0);
                        usuario.LocaisSelecionados = list.Cast<int>().ToArray();
                    }

                    var usuarioDomain = Mapper.Map<UsuarioViewModel, Usuario>(usuario);
                    _usuarioApp.Update(usuarioDomain);
                    this.Obter(usuarioDomain);

                    return RedirectToAction("Index");
                }

                return View(usuario);
            }
            catch
            {
                throw;
            }
        }

        // GET: Usuario/Delete/5
        public ActionResult Excluir(int usuarioId)
        {
            return View();
        }


        [HttpPost]
        [ActionName("Excluir")]
        public ActionResult ConfirmaExclusao(int usuarioId)
        {
            try
            {
                //Verifica se existe contratado vinculado à empresa
                Contratado contratadoVinculado = _empresaApp.ObterContratadoVinculado(null, usuarioId);

                if (contratadoVinculado != null)
                {
                    return Json(new { Success = false, contratadoVinculado = true }, JsonRequestBehavior.AllowGet);
                }

                _usuarioApp.RemoveEspecifico(usuarioId);

                return Json(new { Success = true, contratadoExcluido = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Acesso(int usuarioId, string usuarioEmail)
        {
            if (User.Identity.IsAuthenticated)
            {
                var usuario = User.Identity;
                var role = UserManager.GetRoles(usuario.GetUserId()).First();

                ViewBag.Perfil = new SelectList(_perfilApp.GetAll(), "PerfilId", "Nome", role);
                ViewBag.UsuarioId = usuarioId;
            }

            var perfilViewModel = Mapper.Map<IEnumerable<Perfil>, IEnumerable<PerfilViewModel>>(_perfilApp.GetAll());

            return View(perfilViewModel);
        }

        [HttpPost]
        public ActionResult Acesso(int usuarioId, string perfil, string email)
        {
            string perfilTrocado = "Terceiro";

            if (User.Identity.IsAuthenticated)
            {
                //O próprio usuário está trocando seu perfil
                if (User.Identity.GetUserName().Equals(email))
                {
                    perfilTrocado = "Próprio";
                }

                var userChange = UserManager.Users.Where(u => u.Email.Equals(email)).FirstOrDefault();

                //var usuario = User.Identity;
                var role = UserManager.GetRoles(userChange.Id).FirstOrDefault();

                VerificaPerfil(perfil);

                if (role != null)
                {
                    if (role.Count() > 0)
                    {
                        if (perfil != role)
                        {
                            //Altera perfil do usuário                        
                            UserManager.RemoveFromRole(userChange.Id, role);
                            UserManager.AddToRole(userChange.Id, perfil);

                            AtualizaPerfil(perfil, usuarioId);                            
                        }
                    }
                }
                else
                {
                    //Adiciona perfil ao novo usuário
                    var result = UserManager.AddToRole(userChange.Id, perfil);

                    AtualizaPerfil(perfil, usuarioId);
                }
            }

            return Json(new { success = true, perfil = true, usuario = perfilTrocado }, JsonRequestBehavior.AllowGet); ;
        }

        private void AtualizaPerfil(string perfil, int usuarioId)
        {
            var usuario = _usuarioApp.GetById(usuarioId);
            usuario.Perfil = perfil;
            _usuarioApp.Update(usuario);
        }

        private void VerificaPerfil(string perfil)
        {
            // Create Admin Role
            string roleName = perfil;

            // Check to see if Role Exists, if not create it
            if (!RoleManager.RoleExists(roleName))
            {
                var roleResult = RoleManager.Create(new IdentityRole(roleName));
            }
        }

        private void Obter(Usuario usuario)
        {
            var updateUsuario = _usuarioApp.ObterLocaisPorId(usuario);

            int[] myIndices = new int[usuario.LocaisSelecionados.Count()];

            List<int> list = new List<int>();

            foreach (var selecao in usuario.LocaisSelecionados)
            {
                list.Add(selecao);

            }
            updateUsuario.LocaisSelecionados = list.Cast<int>().ToArray();


            _usuarioApp.SalvarLocaisSelecionados(updateUsuario);
        }

        private void UpdateUsuarioLocais(int[] selecionados, Usuario updateUsuario)
        {
            if (selecionados == null)
            {
                updateUsuario.MappedLocal = new List<MappedLocal>();
                return;
            }

            var selecionadosX = new HashSet<int>(selecionados);
            var usuarioLocais = new HashSet<int>(updateUsuario.MappedLocal.Select(m => m.MappedLocalId));

            foreach (var local in _mappedLocalApp.GetAll())
            {
                if (selecionadosX.Contains(local.MappedLocalId))
                {
                    if (!usuarioLocais.Contains(local.MappedLocalId))
                    {
                        updateUsuario.MappedLocal.Add(local);
                    }
                }
                else
                {
                    if (usuarioLocais.Contains(local.MappedLocalId))
                    {
                        updateUsuario.MappedLocal.Remove(local);
                    }
                }
            }
        }

        [HttpPost]
        [ActionName("VerificaUsuarioCadastrado")]
        public ActionResult VerificaUsuarioCadastrado(string login, int UsuarioId)
        {
            try
            {
                var usuario = new Usuario();

                if (usuario != null)
                {
                    return Json(new { success = true, LoginCadastrado = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, LoginCadastrado = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet); ;
            }
        }
    }
}