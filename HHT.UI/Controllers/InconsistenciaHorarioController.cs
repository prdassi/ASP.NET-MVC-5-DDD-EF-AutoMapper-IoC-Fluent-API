using AutoMapper;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Infra.CrossCutting.Helper;
using HHT.UI.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HHT.UI.Controllers
{
    [Authorize]
    public class InconsistenciaHorarioController : Controller
    {
        private readonly ILocalAppService _localApp;
        private readonly IContratadoAppService _contratadoApp;
        private readonly IPontoAppService _pontoApp;
        private readonly IEmpresaAppService _empresaApp;
        private readonly IUsuarioAppService _usuarioApp;

        public InconsistenciaHorarioController(ILocalAppService localApp, IContratadoAppService contratadoApp, IPontoAppService pontoApp,
                                               IEmpresaAppService empresaApp, IUsuarioAppService usuarioApp)
        {
            _localApp = localApp;
            _contratadoApp = contratadoApp;
            _pontoApp = pontoApp;
            _empresaApp = empresaApp;
            _usuarioApp = usuarioApp;
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

        // GET: InconsistenciaHorario
        [Authorize(Roles = "Seguranca, Funcional")]
        public ActionResult Index()
        {
            var locais = _localApp.GetAll().ToList();
            if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
            {
                locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)
            }

            ViewBag.LocalId = new SelectList(locais, "LocalId", "Nome");

            ViewBag.AnoId = new SelectList(FormatarAnoMesDia.Ano(), "Key", "Value");
           
            return View();
        }

        public JsonResult ListaEmpresas(int localId)
        {
            var listaEmpresas = _empresaApp.ObterPorLocal(localId).OrderBy(x => x.Nome);

            var empresas = listaEmpresas.Select(x => new
            {
                empresaId = x.EmpresaId,
                nome = x.Nome
            });

            return Json(empresas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaMeses(int ano)
        {
            return Json(FormatarAnoMesDia.Mes(ano), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RetornoBuscar(int localId, int empresaId, int ano, int mes)
        {
            var locais = _localApp.GetAll().ToList();
            if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
            {
                locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)
            }

            ViewBag.LocalId = new SelectList(locais, "LocalId", "Nome");

            ViewBag.AnoId = new SelectList(FormatarAnoMesDia.Ano(), "Key", "Value");

            var pontoViewModel = Mapper.Map<IEnumerable<Ponto>, IEnumerable<PontoViewModel>>(_pontoApp.ObterInconsistencias(localId, empresaId, null, ano, mes, null, UsuarioLogado().UsuarioId));

            return View("Index", pontoViewModel);
        }

        [HttpPost]
        public ActionResult Buscar(int localId, int empresaId, int ano, int mes)
        {
            var locais = _localApp.GetAll().ToList();
            if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
            {
                locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)
            }

            ViewBag.LocalId = new SelectList(locais, "LocalId", "Nome");

            ViewBag.AnoId = new SelectList(FormatarAnoMesDia.Ano(), "Key", "Value");

            var pontoViewModel = Mapper.Map<IEnumerable<Ponto>, IEnumerable<PontoViewModel>>(_pontoApp.ObterInconsistencias(localId, empresaId, null, ano, mes, null, UsuarioLogado().UsuarioId));

            return View("Index", pontoViewModel);
        }

        private UsuarioViewModel UsuarioLogado()
        {
            string role = string.Empty;
            string userName = string.Empty;

            if (User.Identity.IsAuthenticated)
            {
                role = UserManager.GetRoles(User.Identity.GetUserId()).First();
                userName = User.Identity.GetUserName();
            }

            return Mapper.Map<Usuario, UsuarioViewModel>(_usuarioApp.ObterUsuarioLogado(userName));
        }
    }
}