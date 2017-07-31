using AutoMapper;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Infra.CrossCutting.Helper;
using HHT.Services.Mesnsagem;
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
    public class HHTController : Controller
    {
        private readonly ILocalAppService _localApp;
        private readonly IContratadoAppService _contratadoApp;
        private readonly IPontoAppService _pontoApp;
        private readonly IUsuarioAppService _usuarioApp;
       
        public HHTController(ILocalAppService localApp, IContratadoAppService contratadoApp, IPontoAppService pontoApp, IUsuarioAppService usuarioApp)
        {
            _localApp = localApp;
            _contratadoApp = contratadoApp;
            _pontoApp = pontoApp;
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

        // GET: HHT
        public ActionResult Index()
        {
            try
            {
                string role = string.Empty;
                string userName = string.Empty;

                if (User.Identity.IsAuthenticated)
                {
                    role = UserManager.GetRoles(User.Identity.GetUserId()).First();
                    userName = User.Identity.GetUserName();
                }

                var contratadoViewModel = Mapper.Map<IEnumerable<Local>, IEnumerable<LocalViewModel>>(_localApp.ObterPorPerfil(role, userName));

                ViewBag.LocalId = new SelectList(contratadoViewModel, "LocalId", "Nome");

                return View();
            }
            catch (Exception)
            {
                throw;
            }            
        }

        [HttpGet]
        public ActionResult RegistrarPonto(string registrarPonto, int localId, string localNome)
        {
            if (registrarPonto.Equals("Saida"))
            {
                ViewBag.RegistrarPonto = "Saída";
            }
            else
            {
                ViewBag.RegistrarPonto = registrarPonto;
            }

            ViewBag.LocalId = localId;
            ViewBag.LocalNome = localNome;

            return View();
        }

        public ActionResult ConfirmarRegistro(ContratadoViewModel contratado, string registrarPonto, int localId, string localNome)
        {
            if (registrarPonto.Equals("Saida"))
            {
                ViewBag.RegistrarPonto = "Saída";
            }
            else
            {
                ViewBag.RegistrarPonto = registrarPonto;
            }
            ViewBag.LocalId = localId;
            ViewBag.LocalNome = localNome;

            //Redirecionado da Action - ConfirmarPorRG
            ContratadoViewModel contratadoTempData = (ContratadoViewModel)TempData["contratadoViewModel"];

            if ((!String.IsNullOrEmpty(contratado.RG) && String.IsNullOrEmpty(contratado.Nome)) || contratadoTempData != null)
            {
                if (contratadoTempData != null)
                {
                    contratado.RG = contratadoTempData.RG;
                    ViewBag.LocalNome = TempData["localNome"].ToString();
                }

                var contratadoViewModel = Mapper.Map<Contratado, ContratadoViewModel>(_contratadoApp.ObterPorRG(localId, contratado.RG));

                TempData["itemProurado"] = "RG";

                if (contratadoViewModel != null)
                {
                    return View(contratadoViewModel);
                }

                return View("RegistrarPonto", contratadoViewModel);
            }
            else if (String.IsNullOrEmpty(contratado.RG) || !String.IsNullOrEmpty(contratado.Nome))
            {
                var contratadoViewModel = Mapper.Map<IEnumerable<Contratado>, IEnumerable<ContratadoViewModel>>(_contratadoApp.ObterPorNome(localId, contratado.Nome));

                TempData["itemProurado"] = "Nome";

                return View("RegistrarPonto", contratadoViewModel);
            }

            return View();
        }

        public ActionResult ConfirmarPorRG(string rg, string registrarPonto, int localId, string localNome)
        {
            ContratadoViewModel contratadoViewModel = new ContratadoViewModel();
            contratadoViewModel.RG = rg.ToString();

            TempData["contratadoViewModel"] = contratadoViewModel;
            TempData["localNome"] = localNome.ToString(); //Todo: Problema com acentuação

            return RedirectToAction("ConfirmarRegistro", new { contratado = "", registrarPonto = registrarPonto, localId = localId, localNome = localNome });
        }

        [HttpPost]
        public ActionResult SalvarRegistro(ContratadoViewModel contratado, DateTime DataRegistroEntrada, string registro, int localId, string localNome)
        {
            PontoViewModel pontoViewModel = new PontoViewModel();
            pontoViewModel.ContratadoId = contratado.ContratadoId;
            pontoViewModel.DataRegistro = DataRegistroEntrada;
            pontoViewModel.HoraRegistro = DataRegistroEntrada;
            pontoViewModel.Registro = registro;
            pontoViewModel.LocalId = localId;
            pontoViewModel.ModoRegistro = Enumerador.ModoRegisto.Automático.ToString();
            pontoViewModel.UsuarioId = UsuarioLogado().UsuarioId;

            var pontoDomain = Mapper.Map<PontoViewModel, Ponto>(pontoViewModel);
            _pontoApp.Add(pontoDomain);

            if (registro.Equals("Saída"))
            {
                registro = Enumerador.Registo.Saida.ToString();
            }
            else
            {
                registro = Enumerador.Registo.Entrada.ToString();
            }

            return RedirectToActionPermanent("RegistrarPonto", new { registrarPonto = registro, localId = localId, localNome = localNome }).Mensagem("Registrado inserido com sucesso!", "Sucesso");
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