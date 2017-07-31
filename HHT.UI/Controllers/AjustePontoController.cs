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
    public class AjustePontoController : Controller
    {
        private readonly ILocalAppService _localApp;
        private readonly IContratadoAppService _contratadoApp;
        private readonly IPontoAppService _pontoApp;
        private readonly IEmpresaAppService _empresaApp;
        private readonly IAjustePontoAppService _ajustePontoApp;
        private readonly IUsuarioAppService _usuarioApp;

        public AjustePontoController(ILocalAppService localApp, IContratadoAppService contratadoApp, IPontoAppService pontoApp,
                                     IEmpresaAppService empresaApp, IAjustePontoAppService ajustePontoApp, IUsuarioAppService usuarioApp)
        {
            _localApp = localApp;
            _contratadoApp = contratadoApp;
            _pontoApp = pontoApp;
            _empresaApp = empresaApp;
            _ajustePontoApp = ajustePontoApp;
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

        // GET: AjustesPonto
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
            var listaEmpresas = _empresaApp.ObterPorLocal(localId);

            var empresas = listaEmpresas.Select(x => new
            {
                empresaId = x.EmpresaId,
                nome = x.Nome
            });

            return Json(empresas, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaContratados(int empresaId)
        {
            var listaContratados = _contratadoApp.ObterPorEmpresa(empresaId);

            var contratados = listaContratados.Select(x => new
            {
                contratadoId = x.ContratadoId,
                nome = x.Nome
            });

            return Json(contratados, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaMeses(int ano)
        {
            return Json(FormatarAnoMesDia.Mes(ano), JsonRequestBehavior.AllowGet);
        }

        public JsonResult ListaDias(int mes, int ano)
        {
            if (mes > 0)
            {
                return Json(FormatarAnoMesDia.Dia(mes, ano), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Buscar(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        {
            var locais = _localApp.GetAll().ToList();
            if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
            {
                locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)
            }

            ViewBag.LocalId = new SelectList(locais, "LocalId", "Nome", "-- Selecione --");
            ViewBag.AnoId = new SelectList(FormatarAnoMesDia.Ano(), "Key", "Value", "-- Selecione --");
            ViewBag.Dia = dia; //Identificar quando será apresentado o mês inteiro ou apenas um dia específico

            var pontoViewModel = Mapper.Map<IEnumerable<Ponto>, IEnumerable<PontoViewModel>>(_pontoApp.ObterPorFiltro(localId, empresaId, contratadoId, ano, mes, dia));

            return View("Index", pontoViewModel);
        }

        //[HttpPost]
        public ActionResult AjustarPonto(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        {
            var ajustePontoViewModel = Mapper.Map<List<AjustePonto>, List<AjustePontoViewModel>>(_ajustePontoApp.ObterPontoDetalhado(localId, empresaId, contratadoId, ano, mes, dia));

            ViewBag.DiaBuscado = dia;

            return View(ajustePontoViewModel);
        }
        public ActionResult Editar(int contratadoId, string nome, string rg, int ano, string mes, int dia, int diaBuscado, string origem = "")
        {
            List<SelectListItem> tipo = new List<SelectListItem>();
            tipo.Add(new SelectListItem() { Text = "Entrada", Value = "Entrada", Selected = false });
            tipo.Add(new SelectListItem() { Text = "Saída", Value = "Saída", Selected = false });

            ViewBag.Registro = new SelectList(tipo, "Value", "Text");

            int numeroMes = int.TryParse(mes, out numeroMes) ? numeroMes : FormatarAnoMesDia.NumeroMes(mes);

            var pontoViewModel = Mapper.Map<IEnumerable<Ponto>, IEnumerable<PontoViewModel>>(_pontoApp.ObterRegistroDia(contratadoId, ano, numeroMes, dia));

            ViewBag.LocalId = pontoViewModel.First().LocalId;
            ViewBag.EmpresaId = pontoViewModel.First().Contratado.EmpresaId;
            ViewBag.ContratadoId = contratadoId;
            ViewBag.Ano = ano;
            ViewBag.Mes = numeroMes;
            ViewBag.Dia = diaBuscado;
            ViewBag.Origem = origem;

            return View(pontoViewModel);
        }

        [HttpPost]
        public ActionResult Cadastrar(PontoViewModel ponto, int diaBuscado)
        {
            PontoViewModel pontoViewModel = new PontoViewModel();
            pontoViewModel.ContratadoId = ponto.ContratadoId;

            DateTime dia = ponto.DataRegistro;
            TimeSpan hora = TimeSpan.Parse(ponto.HoraRegistro.Value.ToLongTimeString());

            pontoViewModel.DataRegistro = dia.Date + hora;
            pontoViewModel.HoraRegistro = dia.Date + hora;
            pontoViewModel.Registro = ponto.Registro;
            pontoViewModel.LocalId = ponto.LocalId;
            pontoViewModel.ModoRegistro = Enumerador.ModoRegisto.Manual.ToString();
            pontoViewModel.Justificativa = ponto.Justificativa;
            pontoViewModel.UsuarioId = UsuarioLogado().UsuarioId;

            var pontoDomain = Mapper.Map<PontoViewModel, Ponto>(pontoViewModel);
            _pontoApp.Add(pontoDomain);

            return RedirectToActionPermanent("Editar", new { contratadoId = ponto.ContratadoId, nome = ponto.Contratado.Nome, rg = ponto.Contratado.RG, ano = ponto.DataRegistro.Year, mes = FormatarAnoMesDia.MesPorExtenso(ponto.DataRegistro.Month), dia = ponto.DataRegistro.Day, diaBuscado = diaBuscado });
        }

        [HttpPost]
        [ActionName("Excluir")]
        public ActionResult ConfirmaExclusao(int pontoId)
        {
            try
            {
                var ponto = _pontoApp.GetById(pontoId);

                _pontoApp.Remove(ponto);

                return Json(new { Success = true, pontoExcluido = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AlterarPonto(int pontoId, string registro, string hora, string justificativa)
        {
            try
            {
                var ponto = _pontoApp.GetById(pontoId);

                Ponto alterarPonto = new Ponto();

                alterarPonto = ponto;
                alterarPonto.Registro = registro;
                alterarPonto.HoraRegistro = ponto.DataRegistro.Date + TimeSpan.Parse(hora);
                alterarPonto.Justificativa = justificativa;
                alterarPonto.UsuarioId = UsuarioLogado().UsuarioId;

                _pontoApp.Update(alterarPonto);

                return Json(new { Success = true, pontoAlterado = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return View();
            }
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