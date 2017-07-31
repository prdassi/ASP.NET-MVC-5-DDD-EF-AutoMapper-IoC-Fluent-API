using AutoMapper;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Infra.CrossCutting.Helper;
using HHT.UI.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace HHT.UI.Controllers
{
    [Authorize]
    public class ContratadoController : Controller
    {
        private readonly IEmpresaAppService _empresaApp;
        private readonly ILocalAppService _localApp;
        private readonly IServicoAppService _servicoApp;
        private readonly IDocumentoGeralAppService _documentoGeralApp;
        private readonly IUsuarioAppService _usuarioApp;
        private readonly IMappedServicoAppService _mappedServicoApp;

        private readonly IFuncaoAppService _funcaoApp;
        private readonly IContratadoAppService _contratadoApp;
        private readonly IArquivoContratadoAppService _arquivoContratadoApp;
        private readonly ITipoDocumentoAppService _tipoDocumentoApp;
        private readonly IHistoricoAppService _historicoApp;
        private readonly IPontoAppService _pontoApp;

        public ContratadoController(IEmpresaAppService empresaApp, ILocalAppService localApp, IServicoAppService servicoApp,
            IDocumentoGeralAppService documentoGeralApp, IArquivoContratadoAppService arquivoContratadoApp,
            IUsuarioAppService usuarioApp, IMappedServicoAppService mappedServicoApp, IContratadoAppService contratadoApp,
            IFuncaoAppService funcaoApp, ITipoDocumentoAppService tipoDocumentoApp, IHistoricoAppService historicoApp,
            IPontoAppService pontoApp)
        {
            _empresaApp = empresaApp;
            _localApp = localApp;
            _servicoApp = servicoApp;
            _documentoGeralApp = documentoGeralApp;
            _arquivoContratadoApp = arquivoContratadoApp;
            _usuarioApp = usuarioApp;
            _mappedServicoApp = mappedServicoApp;
            _contratadoApp = contratadoApp;
            _funcaoApp = funcaoApp;
            _tipoDocumentoApp = tipoDocumentoApp;
            _historicoApp = historicoApp;
            _pontoApp = pontoApp;
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

        // GET: Contratado
        public ActionResult Index()
        {
            string role = string.Empty;
            string userName = string.Empty;

            if (User.Identity.IsAuthenticated)
            {
                role = UserManager.GetRoles(User.Identity.GetUserId()).First();
                userName = User.Identity.GetUserName();

                if (role.Equals("Empresa"))
                {
                    ViewBag.DisplayMenu = true;
                }
            }

            var contratadoViewModel = Mapper.Map<IEnumerable<Contratado>, IEnumerable<ContratadoViewModel>>(_contratadoApp.ObterPorPerfil(role, userName));

            return View(contratadoViewModel);
        }

        public ActionResult Cadastrar()
        {
            ViewBag.Funcoes = new SelectList(_funcaoApp.GetAll().OrderBy(x => x.Nome), "FuncaoId", "Nome");
            ViewBag.Empresas = new SelectList(_empresaApp.GetAll().OrderBy(x => x.Nome), "EmpresaId", "Nome");
            ViewBag.Usuarios = new SelectList(_usuarioApp.GetAll().OrderBy(x => x.Nome), "UsuarioId", "Nome");

            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(ContratadoViewModel contratado)
        {
            try
            {
                var contratadoDomain = Mapper.Map<ContratadoViewModel, Contratado>(contratado);
                _contratadoApp.Add(contratadoDomain);

                _historicoApp.Adicionar(ObterHistorico("ContratadoCriado", contratadoDomain));

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult Editar(int contratadoId)
        {
            var contratado = _contratadoApp.GetById(contratadoId);
            var contratadoViewModel = Mapper.Map<Contratado, ContratadoViewModel>(contratado);

            ViewBag.Funcoes = new SelectList(_funcaoApp.GetAll(), "FuncaoId", "Nome", contratadoViewModel.FuncaoId);
            ViewBag.Empresas = new SelectList(_empresaApp.GetAll(), "EmpresaId", "Nome", contratadoViewModel.EmpresaId);
            ViewBag.Usuarios = new SelectList(_usuarioApp.GetAll(), "UsuarioId", "Nome", contratadoViewModel.UsuarioId);

            return View(contratadoViewModel);
        }

        [HttpPost]
        public ActionResult Editar(ContratadoViewModel contratado)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var contratadoDomain = Mapper.Map<ContratadoViewModel, Contratado>(contratado);
                    _contratadoApp.Update(contratadoDomain);

                    if (!contratado.Restricao)
                    {
                        _arquivoContratadoApp.AjustarVencimentoCurso(null, contratado.ContratadoId);
                    }

                    _historicoApp.Adicionar(ObterHistorico("ContratadoAlterado", contratadoDomain));

                    return RedirectToAction("Index");
                }

                return View(contratado);
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public ActionResult Excluir(int contratadoId)
        {
            return View();
        }

        [Authorize(Roles = "Seguranca, Funcional")]
        [HttpPost]
        [ActionName("Excluir")]
        public ActionResult ConfirmaExclusao(int contratadoId)
        {
            try
            {
                Contratado contratado = _contratadoApp.GetById(contratadoId);

                if (contratado.ArquivosContratado.Count() > 0)
                {
                    return Json(new { Success = false, documentoVinculado = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _historicoApp.RemoveById(contratadoId);

                    _pontoApp.RemoveById(contratadoId);

                    _contratadoApp.Remove(contratado);

                    return Json(new { Success = true, contratadoExcluido = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost]
        [ActionName("ExcluirContratadoCompleto")]
        public ActionResult ConfirmaExclusaoContratadoCompleto(int contratadoId)
        {
            try
            {
                Contratado contratado = _contratadoApp.GetById(contratadoId);

                if (contratado.ArquivosContratado.Count() > 0)
                {
                    _historicoApp.RemoveById(contratadoId);

                    _pontoApp.RemoveById(contratadoId);

                    _arquivoContratadoApp.ExcluirContratadoCompleto(contratado.ArquivosContratado.First().ArquivoContratadoId, contratadoId);

                    return Json(new { success = true, arquivoDocumento = true }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, arquivoDocumento = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }


        public ActionResult Documentos(int contratadoId)
        {
            var contratadoViewModel = Mapper.Map<Contratado, ContratadoViewModel>(_contratadoApp.ObterDocumentos(contratadoId));

            return View(contratadoViewModel);
        }

        public ActionResult ArquivoContratado(int contratadoId)
        {
            try
            {
                var contratado = _contratadoApp.GetById(contratadoId);
                var contratadoViewModel = Mapper.Map<Contratado, ContratadoViewModel>(contratado);

                ArquivoContratadoViewModel arquivoContratadoViewModel = new ArquivoContratadoViewModel();
                arquivoContratadoViewModel.Contratados.Add(contratadoViewModel);

                arquivoContratadoViewModel.DataDocumento = null;

                ViewBag.TiposDocumento = new SelectList(_tipoDocumentoApp.ObterTiposDocumentoContratado(), "TipoDocumentoId", "Nome");
                ViewBag.Locais = new SelectList(_localApp.GetAll(), "LocalId", "Nome");

                TempData["apresentarModal"] = "";

                return View(arquivoContratadoViewModel);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArquivoContratado(ArquivoContratadoViewModel arquivoContratado, int contratadoId, HttpPostedFileBase upload)
        {
            try
            {
                var contratado = _contratadoApp.GetById(contratadoId);
                var contratadoViewModel = Mapper.Map<Contratado, ContratadoViewModel>(contratado);

                if (arquivoContratado.LocalId == 0)
                {
                    arquivoContratado.LocalId = _localApp.GetNull();
                }


                var arquivoContratadoDomain = Mapper.Map<ArquivoContratadoViewModel, ArquivoContratado>(arquivoContratado);
                if (ModelState.IsValid)
                {
                    _arquivoContratadoApp.AdicionaDocumento(arquivoContratadoDomain, contratadoId, upload);

                    var tipoDocumento = _tipoDocumentoApp.GetById(arquivoContratado.TipoDocumentoId);

                    if (tipoDocumento.Nome.Equals("Documento"))
                    {
                        _historicoApp.Adicionar(ObterHistorico("AdicionarDocumento", contratado));
                    }
                    else if (tipoDocumento.Nome.Equals("Integração"))
                    {
                        _historicoApp.Adicionar(ObterHistorico("AdicionarIntegracao", contratado));
                    }
                    else
                    {
                        _historicoApp.Adicionar(ObterHistorico("AdicionarTreinamento", contratado));
                    }

                    if (!String.IsNullOrEmpty(upload.FileName))
                    {
                        _historicoApp.Adicionar(ObterHistorico("AdicionarAnexo", contratado));
                    }
                }

                ArquivoContratadoViewModel arquivoContratadoViewModel = new ArquivoContratadoViewModel();
                arquivoContratadoViewModel.Contratados.Add(contratadoViewModel);

                ViewBag.DocumentosGeral = new SelectList(_documentoGeralApp.ObterDocumentosContratado(), "DocumentoGeralId", "Nome");
                ViewBag.TiposDocumento = new SelectList(_tipoDocumentoApp.ObterTiposDocumentoContratado(), "TipoDocumentoId", "Nome");
                ViewBag.Locais = new SelectList(_localApp.GetAll(), "LocalId", "Nome");

                TempData["apresentarModal"] = "OK";

                return View(arquivoContratadoViewModel);
            }
            catch
            {
                throw;
            }
        }


        [NoAsyncTimeout]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult> DownloadArquivo(int contratadoId, int arquivoContratadoId)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {
                string caminho = ConfigurationManager.AppSettings["PastaUploadContratado"];

                var contratado = _contratadoApp.ObterDocumentos(contratadoId);

                foreach (var documento in contratado.ArquivosContratado)
                {
                    if (documento.ArquivoContratadoId == arquivoContratadoId)
                    {
                        Response.ContentType = "APPLICATION/OCTET-STREAM";
                        String Header = "Attachment; Filename=" + documento.Nome.Split(Convert.ToChar("_"))[0] + documento.Tipo;
                        Response.AppendHeader("Content-Disposition", Header);
                        FileInfo Dfile = new FileInfo(Server.MapPath(caminho + documento.Nome + documento.Tipo));
                        Response.WriteFile(Dfile.FullName);

                        Response.End();
                    }
                }
                return View();

            }
            catch (System.Exception)
            {
                throw;
            }
        }
        // Editar
        public ActionResult EditarArquivo(int contratadoId, int arquivoContratadoId)
        {
            var contratadoViewModel = Mapper.Map<Contratado, ContratadoViewModel>(_contratadoApp.ObterDocumentos(contratadoId));
            var arquivoContratadoViewModel = Mapper.Map<ArquivoContratado, ArquivoContratadoViewModel>(_arquivoContratadoApp.GetById(arquivoContratadoId));
            ViewBag.Nome = contratadoViewModel.Nome;
            ViewBag.RG = contratadoViewModel.RG;
            ViewBag.Funcao = contratadoViewModel.Funcao;
            ViewBag.TiposDocumento = new SelectList(_tipoDocumentoApp.ObterTiposDocumentoContratado(), "TipoDocumentoId", "Nome", arquivoContratadoViewModel.TipoDocumentoId);


            //Regra para o Editar
            List<object> ListaDocumento = new List<object>();
            var documentos = ListaDocumento.Select(x => new { DocumentoGeralId = int.MinValue, Nome = string.Empty });

            List<object> ListaLocais = new List<object>();
            var locais = ListaLocais.Select(x => new { LocalId = int.MinValue, Nome = string.Empty });
            
            var listaDocumentos = _documentoGeralApp.GetAll().Where(d => d.TipoDocumentoId == arquivoContratadoViewModel.TipoDocumentoId && d.Associado.Nome == Enumerador.Associado.Contratado.ToString()).ToList();

            if (arquivoContratadoViewModel.TipoDocumentoId == (int)Enumerador.TipoDocumento.Integração + 1)
            {
                documentos = listaDocumentos.Select(x => new { DocumentoGeralId = x.DocumentoGeralId, Nome = (x.Nome.Contains("Local") ? "Local" + " - " + x.Local.Nome : x.Nome) });
                locais = listaDocumentos.Select(x => new { LocalId = x.Local.LocalId, Nome = x.Local.Nome });
                ViewBag.Locais = new SelectList(locais, "LocalId", "Nome", arquivoContratadoViewModel.LocalId);
            }
            else
            {
                documentos = listaDocumentos.Select(x => new { DocumentoGeralId = x.DocumentoGeralId, Nome = x.Nome });
                ViewBag.Locais = new SelectList(_localApp.GetAll(), "LocalId", "Nome", arquivoContratadoViewModel.LocalId);
            }
            var documentosDistintos = documentos.GroupBy(g => g.Nome).Select(s => s.FirstOrDefault()).ToList();
            ViewBag.DocumentosGeral = new SelectList(documentosDistintos, "DocumentoGeralId", "Nome", arquivoContratadoViewModel.DocumentoGeralId);



     
            var documentoSelecionado = contratadoViewModel.ArquivosContratado.Where(a => a.ArquivoContratadoId == arquivoContratadoId).Select(s => s.DocumentoGeral).FirstOrDefault();


            foreach (var item in contratadoViewModel.ArquivosContratado)
            {
                if (item.ArquivoContratadoId == arquivoContratadoId)
                {
                    contratadoViewModel.ArquivosContratado.Clear();
                    contratadoViewModel.ArquivosContratado.Add(item);
                    break;
                }
            }

            return View(arquivoContratadoViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarArquivo(ArquivoContratadoViewModel arquivoContratado, int contratadoId, HttpPostedFileBase upload)
        {
            try
            {                
                if (arquivoContratado.LocalId == 0)
                {
                    arquivoContratado.LocalId = _localApp.GetNull();
                }

                var arquivoContratadoDomain = Mapper.Map<ArquivoContratadoViewModel, ArquivoContratado>(arquivoContratado);

                _arquivoContratadoApp.Update(arquivoContratadoDomain);

                _arquivoContratadoApp.EditarDocumento(arquivoContratadoDomain, upload, contratadoId);

                var contratado = _contratadoApp.GetById(contratadoId);
                var tipoDocumento = _tipoDocumentoApp.GetById(arquivoContratado.TipoDocumentoId);

                if (tipoDocumento.Nome.Equals("Documento"))
                {
                    _historicoApp.Adicionar(ObterHistorico("AlterarDocumento", contratado));
                }
                else if (tipoDocumento.Nome.Equals("Integração"))
                {
                    _historicoApp.Adicionar(ObterHistorico("AlterarIntegracao", contratado));
                }
                else
                {
                    _historicoApp.Adicionar(ObterHistorico("AlterarTreinamento", contratado));
                }

                if (upload != null)
                {
                    if (!String.IsNullOrEmpty(upload.FileName))
                    {
                        _historicoApp.Adicionar(ObterHistorico("AlterarAnexo", contratado));
                    }
                }

                return RedirectToAction("Documentos", new { contratadoId = contratadoId });
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ExcluirDocumento(int arquivoContratadoId, int contratadoId)
        {
            try
            {
                _arquivoContratadoApp.ExcluirDocumento(arquivoContratadoId, contratadoId);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Seguranca, Funcional")]
        public ActionResult GerarQRCode(int contratadoId)
        {
            ExcluirQRCodePasta();

            IdentificacaoViewModel empresaViewModel = Mapper.Map<Identificacao, IdentificacaoViewModel>(_contratadoApp.Identificacao(contratadoId));

            return View(empresaViewModel);
        }

        [HttpPost]
        public ActionResult ExcluirQRCodePasta()
        {
            try
            {
                string caminho = Server.MapPath(ConfigurationManager.AppSettings["PastaQRCode"]);

                // Obtem todos os arquivos da pasta
                var files = Directory.GetFiles(caminho);
                foreach (var f in files)
                {
                    // Apaga arquivo
                    System.IO.File.Delete(f);
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public ActionResult Historico(int contratadoId)
        {
            var historicoViewModel = Mapper.Map<IEnumerable<Historico>, IEnumerable<HistoricoViewModel>>(_historicoApp.ObterPorId(contratadoId));

            return View(historicoViewModel);
        }

        private Historico ObterHistorico(string acao, Contratado contratado)
        {
            Historico historico = new Historico();
            historico.Data = DateTime.Now;
            historico.Contradado = new Contratado();
            historico.Contradado = contratado;
            historico.UsuarioId = UsuarioLogado().UsuarioId;

            switch (acao)
            {
                case "ContratadoCriado":
                    historico.Descricao = ConfigurationManager.AppSettings["ContratadoCriado"];
                    break;
                case "ContratadoAlterado":
                    historico.Descricao = ConfigurationManager.AppSettings["ContratadoAlterado"];
                    break;
                case "AdicionarTreinamento":
                    historico.Descricao = ConfigurationManager.AppSettings["AdicionadoTreinamento"];
                    break;
                case "AlterarTreinamento":
                    historico.Descricao = ConfigurationManager.AppSettings["AlteradoTreinamento"];
                    break;
                case "AdicionarDocumento":
                    historico.Descricao = ConfigurationManager.AppSettings["AdicionadoDocumento"];
                    break;
                case "AlterarDocumento":
                    historico.Descricao = ConfigurationManager.AppSettings["AlteradoDocumento"];
                    break;
                case "AdicionarIntegracao":
                    historico.Descricao = ConfigurationManager.AppSettings["AdicionadoIntegracao"];
                    break;
                case "AlterarIntegracao":
                    historico.Descricao = ConfigurationManager.AppSettings["AlteradoIntegracao"];
                    break;
                case "AdicionarAnexo":
                    historico.Descricao = ConfigurationManager.AppSettings["AdicionadoAnexo"];
                    break;
                case "AlterarAnexo":
                    historico.Descricao = ConfigurationManager.AppSettings["AlteradoAnexo"];
                    break;
                default:
                    Console.WriteLine("Não encontrado");
                    break;
            }

            return historico;
        }

        public JsonResult ListaDocumentos(int tipoDocumentoId)
        {
            var listaDocumentos = _documentoGeralApp.GetAll().Where(d => d.TipoDocumentoId == tipoDocumentoId && d.Associado.Nome == Enumerador.Associado.Contratado.ToString()).ToList();

            //Lista e objeto generico para exibir os documentos

            List<object> Lista = new List<object>();
            var documentos = Lista.Select(x => new { DocumentoGeralId = int.MinValue, Nome = string.Empty });
            if (tipoDocumentoId == (int)Enumerador.TipoDocumento.Integração + 1)
            {
                documentos = listaDocumentos.Select(x => new { DocumentoGeralId = x.DocumentoGeralId, Nome = (x.Nome.Contains("Local") ? "Local" + " - " + x.Local.Nome : x.Nome) });

            }
            else
            {
                documentos = listaDocumentos.Select(x => new { DocumentoGeralId = x.DocumentoGeralId, Nome = x.Nome });
            }

            var documentosDistintos = documentos.GroupBy(g => g.Nome).Select(s => s.FirstOrDefault()).ToList();
            return Json(documentosDistintos, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LocalDocumentos(int DocumentoGeralId)
        {
            var listaDocumentos = _documentoGeralApp.GetAll().Where(d => d.DocumentoGeralId == DocumentoGeralId).ToList();
            var documentosLocais = listaDocumentos.Select(x => new { LocalId = x.Local.LocalId, Nome = x.Local.Nome });


            return Json(documentosLocais, JsonRequestBehavior.AllowGet);
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