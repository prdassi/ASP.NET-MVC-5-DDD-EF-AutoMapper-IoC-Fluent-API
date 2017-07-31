using AutoMapper;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Infra.CrossCutting.Helper;
using HHT.UI.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HHT.UI.Controllers
{
    [Authorize]
    public class EmpresaController : Controller
    {
        private readonly IEmpresaAppService _empresaApp;
        private readonly ILocalAppService _localApp;
        private readonly IServicoAppService _servicoApp;
        private readonly IDocumentoGeralAppService _documentoGeralApp;
        private readonly IArquivoEmpresaAppService _arquivoEmpresaApp;
        private readonly IUsuarioAppService _usuarioApp;
        private readonly IMappedServicoAppService _mappedServicoApp;

        public EmpresaController(IEmpresaAppService empresaApp, ILocalAppService localApp, IServicoAppService servicoApp,
            IDocumentoGeralAppService documentoGeralApp, IArquivoEmpresaAppService arquivoEmpresaApp,
            IUsuarioAppService usuarioApp, IMappedServicoAppService mappedServicoApp)
        {
            _empresaApp = empresaApp;
            _localApp = localApp;
            _servicoApp = servicoApp;
            _documentoGeralApp = documentoGeralApp;
            _arquivoEmpresaApp = arquivoEmpresaApp;
            _usuarioApp = usuarioApp;
            _mappedServicoApp = mappedServicoApp;
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

        // GET: Empresa
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

            var empresaViewModel = Mapper.Map<IEnumerable<Empresa>, IEnumerable<EmpresaViewModel>>(_empresaApp.ObterPorPerfil(role, userName));

            foreach (var item in empresaViewModel)
            {
                var informarDocumento = item.ArquivosEmpresa.Where(a => a.DataDocumento.CompareTo(DateTime.Now) < 0);

                if (informarDocumento.Count() > 0)
                {
                    item.VerificarArquivo = Enumerador.IdentificacaoDocumento.Vencido.ToString();
                }

                informarDocumento = item.ArquivosEmpresa.Where(a => a.DataDocumento.CompareTo(DateTime.Now) >= 0 && (DateTime.Now.AddDays(30).CompareTo(a.DataDocumento) > 0));

                if (informarDocumento.Count() > 0)
                {
                    if (String.IsNullOrEmpty(item.VerificarArquivo))
                    {
                        item.VerificarArquivo = Enumerador.IdentificacaoDocumento.AVencer.ToString();
                    }
                }
            }

            return View(empresaViewModel);
        }

        // GET: Empresa/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Empresa/Cadastrar
        public ActionResult Cadastrar()
        {

            var locais = _localApp.GetAll().ToList();
            if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
            {
                locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)
                ViewBag.Locais = new MultiSelectList(locais, "LocalId", "Nome");
            }

            ViewBag.Servicos = new MultiSelectList(_servicoApp.GetAll().OrderBy(s => s.Nome), "ServicoId", "Nome");

            return View();
        }


        // POST: Empresa/Create
        [Authorize(Roles = "Seguranca, Funcional")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(EmpresaViewModel novaEmpresa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    foreach (var idSelecionado in novaEmpresa.ServicosSelecionados)
                    {
                        var servico = _servicoApp.GetById(idSelecionado);
                        var servicoViewModel = Mapper.Map<Servico, ServicoViewModel>(servico);

                        var mappedServicoViewModel = new MappedServicoViewModel
                        {
                            ServicoId = servicoViewModel.ServicoId,
                            Nome = servicoViewModel.Nome,
                        };
                        novaEmpresa.MappedServico.Add(mappedServicoViewModel);
                    }

                    foreach (var idLocalSelecionado in novaEmpresa.LocaisSelecionados)
                    {
                        var local = _localApp.GetById(idLocalSelecionado);
                        var localViewModel = Mapper.Map<Local, LocalViewModel>(local);

                        var mappedEmpresaLocalViewModel = new MappedEmpresaLocalViewModel
                        {
                            MappedEmpresaLocalId = localViewModel.LocalId,
                            DataCadastro = localViewModel.DataCadastro,
                            Nome = localViewModel.Nome,
                            Sigla = localViewModel.Sigla,
                            LocalId = localViewModel.LocalId
                        };
                        novaEmpresa.MappedEmpresaLocal.Add(mappedEmpresaLocalViewModel);
                    }


                    var usuarioDomain = Mapper.Map<EmpresaViewModel, Empresa>(novaEmpresa);
                    _empresaApp.Add(usuarioDomain);
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }

        }

        // GET: Empresa/Editar/5
        public ActionResult Editar(int empresaId)
        {
            try
            {
                var empresa = _empresaApp.GetById(empresaId);
                var empresaViewModel = Mapper.Map<Empresa, EmpresaViewModel>(empresa);

                empresaViewModel.ValidadeDocumento = _empresaApp.ObterMenorDataValidade(empresaId);

                //  ViewBag.LocalId = new SelectList(_localApp.GetAll(), "LocalId", "Nome", empresaViewModel.LocalId);
                var locais = _localApp.GetAll().ToList();
                if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
                {
                    locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)

                }
                ViewBag.Locais = new MultiSelectList(locais, "LocalId", "Nome", _localApp.ObterLocaisEmpresaSeleciponados(empresa));
                ViewBag.Servicos = new MultiSelectList(_servicoApp.GetAll(), "ServicoId", "Nome", _servicoApp.ObterServicosSelecionados(empresa));

                return View(empresaViewModel);
            }
            catch (System.Exception)
            {

                return View();
            }
        }

        // POST: Empresa/Editar/5
        [Authorize(Roles = "Seguranca, Funcional")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Editar(EmpresaViewModel empresa)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var empresaDomain = Mapper.Map<EmpresaViewModel, Empresa>(empresa);
                    _empresaApp.Update(empresaDomain);

                    this.Obter(empresaDomain);

                   this.ObterLocais(empresaDomain);

                    return RedirectToAction("Index");
                }

                return View(empresa);

            }
            catch
            {
                return View();
            }
        }

        // GET: Empresa/Delete/5
        public ActionResult Excluir(int empresaId)
        {
            return View();
        }


        // POST: Empresa/Excluir/5
        [Authorize(Roles = "Seguranca, Funcional")]
        [HttpPost]
        [ActionName("Excluir")]        
        public ActionResult ConfirmaExclusao(int empresaId)
        {
            try
            {
                //Verifica se existe usuário vinculado à empresa
                Usuario usuarioVinculado = _empresaApp.ObterUsuarioVinculado(empresaId);
                //Verifica se existe contratado vinculado à empresa
                Contratado contratadoVinculado = _empresaApp.ObterContratadoVinculado(empresaId, null);

                if (usuarioVinculado != null && contratadoVinculado != null)
                {
                    return Json(new { Success = false, usuarioEContratadoVinculado = true }, JsonRequestBehavior.AllowGet);
                }
                else if (usuarioVinculado != null)
                {
                    return Json(new { Success = false, usuarioVinculado = true }, JsonRequestBehavior.AllowGet);
                }
                else if (contratadoVinculado != null)
                {
                    return Json(new { Success = false, contratadoVinculado = true }, JsonRequestBehavior.AllowGet);
                }
                   
                Empresa empresa = _empresaApp.GetById(empresaId);

                if (empresa.ArquivosEmpresa.Count() > 0)
                {
                    return Json(new { Success = false, documentoVinculado = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    _empresaApp.RemoverEspecifico(empresaId);

                    return Json(new { Success = true, empresaExcluida = true}, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                throw;
            }
        }


        [HttpPost]
        [ActionName("ExcluirEmpresaCompleta")]
        public ActionResult ConfirmaExclusaoEmpresaCompleta(int empresaId)
        {
            try
            {
                Empresa empresa = _empresaApp.GetById(empresaId);

                if (empresa.ArquivosEmpresa.Count() > 0)
                {
                    
                    _arquivoEmpresaApp.ExcluirEmpresaCompleta(empresa.ArquivosEmpresa.First().ArquivoEmpresaId, empresaId);
                   

                    return Json(new { success = true, arquivoDocumento = true }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { success = false, arquivoDocumento = false }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                throw;
            }
        }


        public ActionResult Documentos(int empresaId)
        {
            var empresaViewModel = Mapper.Map<Empresa, EmpresaViewModel>(_empresaApp.ObterDocumento(empresaId));

            return View(empresaViewModel);
        }
        
        public ActionResult ArquivoEmpresa(int empresaId)
        {
            try
            {
                var empresa = _empresaApp.GetById(empresaId);
                var empresaViewModel = Mapper.Map<Empresa, EmpresaViewModel>(empresa);
                
                ViewBag.DocumentosGeral = new SelectList(_documentoGeralApp.ObterDocumentosRestantes(empresaId), "DocumentoGeralId", "Nome");

                TempData["apresentarModal"] = "";

                return View(empresaViewModel);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ArquivoEmpresa(ArquivoEmpresaViewModel arquivoEmpresa, int empresaId, HttpPostedFileBase upload)
        {
            try
            {
                var empresa = _empresaApp.GetById(empresaId);
                var empresaViewModel = Mapper.Map<Empresa, EmpresaViewModel>(empresa);

                if (ModelState.IsValid)
                {
                    var arquivoEmpresaDomain = Mapper.Map<ArquivoEmpresaViewModel, ArquivoEmpresa>(arquivoEmpresa);
                    
                    _arquivoEmpresaApp.AdicionaDocumento(arquivoEmpresaDomain, empresaId, upload);
                }

                ViewBag.DocumentosGeral = new SelectList(_documentoGeralApp.ObterDocumentosRestantes(empresaId), "DocumentoGeralId", "Nome");

                TempData["apresentarModal"] = "OK";

                return View(empresaViewModel);
            }
            catch
            {
                throw;
            }
        }


        [NoAsyncTimeout]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult> DownloadArquivo(int empresaId, int arquivoEmpresaId)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            try
            {                
                string caminho = ConfigurationManager.AppSettings["PastaUploadEmpresa"];

                var empresa = _empresaApp.ObterDocumento(empresaId);

                foreach (var documento in empresa.ArquivosEmpresa)
                {
                    if (documento.ArquivoEmpresaId == arquivoEmpresaId)
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

        public ActionResult EditarArquivo(int empresaId, int arquivoEmpresaId)
        {
            var empresaViewModel = Mapper.Map<Empresa, EmpresaViewModel>(_empresaApp.ObterDocumento(empresaId));

            ViewBag.Empresa = empresaViewModel.Nome;
            ViewBag.CNPJ = empresaViewModel.CNPJ;

            var arquivoEmpresaViewModel = Mapper.Map<ArquivoEmpresa, ArquivoEmpresaViewModel>(_arquivoEmpresaApp.GetById(arquivoEmpresaId));

            var documentoSelecionado = empresaViewModel.ArquivosEmpresa.Where(a => a.ArquivoEmpresaId == arquivoEmpresaId).Select(s => s.DocumentosGeral).FirstOrDefault();

            ViewBag.DocumentosGeral = new SelectList(_documentoGeralApp.ObterDocumentosRestantesIncluido(empresaId, arquivoEmpresaId), "DocumentoGeralId", "Nome", documentoSelecionado.DocumentoGeralId);

            foreach (var item in empresaViewModel.ArquivosEmpresa)
            {
                if (item.ArquivoEmpresaId == arquivoEmpresaId)
                {
                    empresaViewModel.ArquivosEmpresa.Clear();
                    empresaViewModel.ArquivosEmpresa.Add(item);
                    break;
                }
            }

            return View(arquivoEmpresaViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarArquivo(ArquivoEmpresaViewModel arquivoEmpresa, int empresaId, HttpPostedFileBase upload)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var arquivoEmpresaDomain = Mapper.Map<ArquivoEmpresaViewModel, ArquivoEmpresa>(arquivoEmpresa);
                    _arquivoEmpresaApp.Update(arquivoEmpresaDomain);

                    _arquivoEmpresaApp.EditarDocumento(arquivoEmpresaDomain, upload);

                    return RedirectToAction("Documentos", new { empresaId = empresaId });
                }

                return View(arquivoEmpresa);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ExcluirDocumento(int arquivoEmpresaId, int empresaId)
        {
            try
            {
                _arquivoEmpresaApp.ExcluirDocumento(arquivoEmpresaId, empresaId);

                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        #region Tratamento dos MultiSelectBox
        private void Obter(Empresa empresa)
        {
            var updateEmresa = _empresaApp.ObterServicosPorId(empresa);

            int[] myIndices = new int[empresa.ServicosSelecionados.Count()];

            List<int> list = new List<int>();

            foreach (var selecao in empresa.ServicosSelecionados)
            {
                list.Add(selecao);

            }
            updateEmresa.ServicosSelecionados = list.Cast<int>().ToArray();

            _empresaApp.SalvarServicosSelecionados(updateEmresa);
        }


        private void ObterLocais(Empresa empresa)
        {
            var updateEmresa = _empresaApp.ObterServicosPorId(empresa);

            int[] myIndices = new int[empresa.LocaisSelecionados.Count()];

            List<int> list = new List<int>();

            foreach (var selecao in empresa.LocaisSelecionados)
            {
                list.Add(selecao);

            }
            updateEmresa.LocaisSelecionados = list.Cast<int>().ToArray();

            _empresaApp.SalvarLocaisSelecionados(updateEmresa);
        }


        private void UpdateEmpresaServicos(int[] selecionados, Empresa updateEmpresa)
        {
            if (selecionados == null)
            {
                updateEmpresa.MappedServico = new List<MappedServico>();
                return;
            }

            var selecionadosX = new HashSet<int>(selecionados);
            var empresaServicos = new HashSet<int>(updateEmpresa.MappedServico.Select(m => m.MappedServicoId));

            foreach (var servico in _mappedServicoApp.GetAll())
            {
                if (selecionadosX.Contains(servico.MappedServicoId))
                {
                    if (!empresaServicos.Contains(servico.MappedServicoId))
                    {
                        updateEmpresa.MappedServico.Add(servico);
                    }
                }
                else
                {
                    if (empresaServicos.Contains(servico.MappedServicoId))
                    {
                        updateEmpresa.MappedServico.Remove(servico);
                    }
                }
            }
        }
        #endregion
    }
}
