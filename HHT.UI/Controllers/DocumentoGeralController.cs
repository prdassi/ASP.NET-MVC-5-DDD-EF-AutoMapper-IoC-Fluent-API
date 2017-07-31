using AutoMapper;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Infra.CrossCutting.Helper;
using HHT.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using HHT.Services.Mesnsagem;

namespace HHT.UI.Controllers
{
    [Authorize]
    public class DocumentoGeralController : Controller
    {
        private readonly ILocalAppService _localApp;
        private readonly IUsuarioAppService _usuarioApp;
        private readonly IEmpresaAppService _empresaApp;
        private readonly IServicoAppService _servicoApp;
        private readonly IMappedLocalAppService _mappedLocalApp;
        private readonly IDocumentoGeralAppService _documentoGeralApp;
        private readonly ITipoDocumentoAppService _tipoDocumentoApp;
        private readonly IAssociadoAppService _associadoApp;
        private readonly IArquivoEmpresaAppService _arquivoEmpresaApp;
        private readonly IArquivoContratadoAppService _arquivoContratadoApp;

        public DocumentoGeralController(ILocalAppService localApp, IUsuarioAppService usuarioApp, IEmpresaAppService empresaApp,
                                    IServicoAppService servicoApp, IMappedLocalAppService mappedLocalApp, IDocumentoGeralAppService documentoGeralApp,
                                    ITipoDocumentoAppService tipoDocumentoApp, IAssociadoAppService associadoApp, IArquivoEmpresaAppService arquivoEmpresaApp,
                                    IArquivoContratadoAppService arquivoContratadoApp)
        {
            _localApp = localApp;
            _usuarioApp = usuarioApp;
            _empresaApp = empresaApp;
            _servicoApp = servicoApp;
            _mappedLocalApp = mappedLocalApp;

            _documentoGeralApp = documentoGeralApp;
            _tipoDocumentoApp = tipoDocumentoApp;
            _associadoApp = associadoApp;
            _arquivoEmpresaApp = arquivoEmpresaApp;
            _arquivoContratadoApp = arquivoContratadoApp;
        }

        // GET: DocumentoGeral
        public ActionResult Index()
        {
            var documentoGeralViewModel = Mapper.Map<IEnumerable<DocumentoGeral>, IEnumerable<DocumentoGeralViewModel>>(_documentoGeralApp.GetAll());

            return View(documentoGeralViewModel);
        }

        public ActionResult Cadastrar()
        {
            var locais = _localApp.GetAll().ToList();
            if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
            {
                locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)
            }

            ViewBag.LocalId = new SelectList(locais, "LocalId", "Nome");
            ViewBag.TiposDocumentos = new SelectList(_tipoDocumentoApp.GetAll(), "TipoDocumentoId", "Nome");
            ViewBag.Associados = new SelectList(_associadoApp.GetAll(), "AssociadoId", "Nome");

            return View();
        }

        [HttpPost]
        public ActionResult Cadastrar(DocumentoGeralViewModel documento)
        {
            // Verifica quando local não é selecionado e recebe o local defaul(nullo)
            if (documento.LocalId == 0)
            {
                documento.LocalId = _localApp.GetNull();
            }

            var documentoDomain = Mapper.Map<DocumentoGeralViewModel, DocumentoGeral>(documento);
            _documentoGeralApp.Add(documentoDomain);

            return RedirectToAction("Index").Mensagem("Registro inserido com sucesso!","Sucesso");
        }

        public ActionResult Editar(int documentoId)
        {
            var locais = _localApp.GetAll().ToList();
            if (locais.Count() > 0 && String.IsNullOrWhiteSpace(locais.First().Nome))
            {
                locais.RemoveAt(0);  //Remover o item vazio (importante no cadastro de documentos gerais)
            }

            var documento = _documentoGeralApp.GetById(documentoId);
            var documentoGeralViewModel = Mapper.Map<DocumentoGeral, DocumentoGeralViewModel>(documento);

            ViewBag.TiposDocumentos = new SelectList(_tipoDocumentoApp.GetAll(), "TipoDocumentoId", "Nome", documento.TipoDocumentoId);
            ViewBag.Locais = new SelectList(locais, "LocalId", "Nome", documento.LocalId);
            ViewBag.Associados = new SelectList(_associadoApp.GetAll(), "AssociadoId", "Nome", documento.AssociadoId);

            return View(documentoGeralViewModel);
        }

        [HttpPost]
        public ActionResult Editar(DocumentoGeralViewModel documento)
        {
            // Verifica quando local não é selecionado e recebe o local defaul(nullo)
            if (documento.LocalId == 0)
            {
                documento.LocalId = _localApp.GetNull();
            }

            var documentoGeraDomain = Mapper.Map<DocumentoGeralViewModel, DocumentoGeral>(documento);

            _documentoGeralApp.Update(documentoGeraDomain);

            return RedirectToAction("Index");
        }

        // GET: DocumentoGeral/Delete/5
        public ActionResult Excluir()
        {
            return View();
        }


        [HttpPost]
        [ActionName("Excluir")]
        public ActionResult ConfirmaExclusao(int documentoId)
        {
            try
            {
                var arquivoEmpresa = _arquivoEmpresaApp.ObterArquivoPorDocumento(documentoId);
                var arquivoContratado = _arquivoContratadoApp.ObterArquivoPorDocumento(documentoId);

                if (arquivoEmpresa == null && arquivoContratado == null)
                {
                    var documento = _documentoGeralApp.GetById(documentoId);

                    var documentoGeralViewModel = Mapper.Map<DocumentoGeral, DocumentoGeralViewModel>(documento);

                    _documentoGeralApp.Remove(documento);

                    return Json(new { success = true, documentoVinculado = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, documentoVinculado = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
              
                return Json(new { success = false }, JsonRequestBehavior.AllowGet); ;
            }
        }

        [HttpPost]
        [ActionName("DocumentoVinculado")]
        public ActionResult DocumentoVinculado(int documentoId)
        {
            try
            {
                var arquivoEmpresa = _arquivoEmpresaApp.ObterArquivoPorDocumento(documentoId);
                var arquivoContratado = _arquivoContratadoApp.ObterArquivoPorDocumento(documentoId);
                if (arquivoEmpresa == null && arquivoContratado == null)
                {

                    return Json(new { success = true, documentoVinculado = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = true, documentoVinculado = true }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet); ;
            }
        }

        public JsonResult ListaTipoDocumento(string associado)
        {
            var listaDocumentos = _tipoDocumentoApp.GetAll();

            if (associado.Equals(Enumerador.Associado.Empresa.ToString()))
            {
                var documento = listaDocumentos.Where(l => l.Nome == Enumerador.TipoDocumento.Documento.ToString()).ToList();

                return Json(documento, JsonRequestBehavior.AllowGet);
            }

            return Json(listaDocumentos, JsonRequestBehavior.AllowGet);
        }
    }
}