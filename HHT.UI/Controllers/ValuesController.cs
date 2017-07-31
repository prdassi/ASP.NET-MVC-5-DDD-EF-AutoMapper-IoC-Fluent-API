using System;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Infra.CrossCutting.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Results;
using System.Web.Http;
using HHT.UI.ViewModels;
using AutoMapper;
using Newtonsoft.Json;
using System.Web;
using System.Text;

namespace HHT.UI.Controllers
{
    [RoutePrefix("api")]
    public class ValuesController : ApiController
    {
        // GET api/values
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

        public ValuesController(IEmpresaAppService empresaApp, ILocalAppService localApp, IServicoAppService servicoApp,
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

        public ValuesController()
        {
        }

        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // GET api/values/5
        public string GetX(int id)
        {
            return "teste";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // POST api/values
        public void PostCadastro([FromBody]string value)
        {


        }/// <summary>
         /// / api/cadastrarponto/localid/documento/registro
         /// </summary>
         /// <param name="localId"></param>
         /// <param name="documento"></param>
         /// <param name="registro"></param>
         /// <returns></returns>
        [HttpGet]
        [Route("cadastrarponto/{localId}/{documento}/{registro}/{usuario}")]
        public bool CadastrarPonto(int localId, string documento, string registro, int usuario)
        {
            var value = false;
            PontoViewModel pontoViewModel = new PontoViewModel();
            var contratado = _contratadoApp.ObterPorRG(localId, documento);

            if (contratado != null)
            {
                pontoViewModel.ContratadoId = contratado.ContratadoId;
                var data = DateTime.Now;
                pontoViewModel.DataRegistro = data;
                pontoViewModel.HoraRegistro = data;
                pontoViewModel.Registro = registro;
                pontoViewModel.LocalId = localId;
                pontoViewModel.ModoRegistro = Enumerador.ModoRegisto.Automático.ToString();
                pontoViewModel.UsuarioId = usuario;
                var pontoDomain = Mapper.Map<PontoViewModel, Ponto>(pontoViewModel);

                if (contratado.Ativo)
                {
                    if (!contratado.Restricao)
                    {
                        if (contratado.ValidadeDocumento != null)
                        {
                            if (contratado.ValidadeDocumento.Value.CompareTo(DateTime.Now.Date) >= 0)
                            {
                                _pontoApp.Add(pontoDomain);
                                value = true;
                            }
                        }
                        else
                        {
                            if (contratado.ArquivosContratado.Count > 0)
                            {
                                _pontoApp.Add(pontoDomain);
                                value = true;
                            }
                        }
                    }
                }              
            }

            return value;
        }

        [HttpGet]
        [Route("contratadoPorRG/{localId}/{documento}")]
        public List<ContratadoViewModelMobile> ContratadoPorRG(int localId, string documento)
        {
            var appContratados = new ContratadoViewModel();
            
            var appContratado = new ContratadoViewModelMobile();
            var ListContratado = new List<ContratadoViewModelMobile>();
            var contratado = _contratadoApp.ObterPorRG(localId, documento);
            if (contratado != null)
            {
                var appEmpresa = _empresaApp.GetById(contratado.EmpresaId);

                appContratado.Restricao = contratado.Restricao;
                appContratado.ContratadoId = contratado.ContratadoId;
                appContratado.DataAdmissao = contratado.DataAdmissao;
                appContratado.DataCadastro = contratado.DataCadastro;
                appContratado.Nome = contratado.Nome;
                appContratado.RG = contratado.RG;
                appContratado.Empresa = contratado.Empresa.Nome;
                appContratado.EmpresaId = contratado.EmpresaId;
                //appContratado.Funcao = contratado.Funcao.Nome;
                //appContratado.FuncaoId = contratado.FuncaoId;

                ListContratado.Add(appContratado);
            }
            return ListContratado;
        }

        [HttpGet]
        [Route("contratadoPorNome/{localId}/{nome}")]
        public List<ContratadoViewModelMobile> ContratadoPorNome(int localId, string nome)
        {
            var ListAppContratado = new List<ContratadoViewModelMobile>();
            var ListContratado = _contratadoApp.ObterPorNome(localId, nome).ToList();
            if (ListContratado != null)
            {
                foreach (var item in ListContratado)
                {
                    var appContratado = new ContratadoViewModelMobile();
                    var appEmpresa = _empresaApp.GetById(item.EmpresaId);
                    appContratado.Restricao = item.Restricao;
                    appContratado.ContratadoId = item.ContratadoId;
                    appContratado.DataAdmissao = item.DataAdmissao;
                    appContratado.DataCadastro = item.DataCadastro;
                    appContratado.Nome = item.Nome;
                    appContratado.RG = item.RG;
                    appContratado.Empresa = appEmpresa.Nome;
                    appContratado.EmpresaId = appEmpresa.EmpresaId;
                    ListAppContratado.Add(appContratado);
                }
            }

            return ListAppContratado;
        }

        [HttpGet]
        [Route("contratadoPorLocal/{localId}")]
        public List<ContratadoViewModelMobile> ContratadoPorLocal(int localId)
        {
            var ListAppContratado = new List<ContratadoViewModelMobile>();
            var ListContratado = _contratadoApp.ObterPorLocal(localId).ToList();
            if (ListContratado != null)
            {
                foreach (var item in ListContratado)
                {
                    var appContratado = new ContratadoViewModelMobile();
                    var appEmpresa = _empresaApp.GetById(item.EmpresaId);
                    appContratado.Restricao = item.Restricao;
                    appContratado.ContratadoId = item.ContratadoId;
                    appContratado.DataAdmissao = item.DataAdmissao;
                    appContratado.DataCadastro = item.DataCadastro;
                    appContratado.Nome = item.Nome;
                    appContratado.RG = item.RG;
                    appContratado.Empresa = appEmpresa.Nome;
                    appContratado.EmpresaId = appEmpresa.EmpresaId;
                    ListAppContratado.Add(appContratado);
                }
            }

            return ListAppContratado;
        }

        [HttpGet]
        [Route("contratadoPorEmpresa/{empresaId}")]
        public object ListaContratados(int empresaId)
        {
            var listaContratados = _contratadoApp.ObterPorEmpresa(empresaId);
            
            var contratados = listaContratados.Select(x => new
            {
                contratadoId = x.ContratadoId,
                nome = x.Nome
            });
            List<object> Lista = new List<object>();
            Lista.Add(new
            {
                nome = contratados.ToList()[0].nome,
                id = contratados.ToList()[0].contratadoId

            });

            object json = JsonConvert.SerializeObject(Lista[0]);
            //  return Request.CreateResponse(HttpStatusCode.OK, contratados);

            return json;

        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpGet]
        [Route("login/{usuario}/{senha}")]
        public bool Login(string usuario, string senha)
        {
           return _usuarioApp.Login(usuario, senha) != null ? true : false;
        }

        //Buscar os locais por determinado perfil
        [HttpGet]
        [Route("local/{email}")]
        public List<LocalViewModel> GetLocalPerfil(string email)
        {
            var contratadoViewModel = Mapper.Map<List<Local>, List<LocalViewModel>>(_localApp.ObterPorEmail(email));

            return contratadoViewModel;
        }

        [HttpGet]
        [Route("local")]
        public List<Local> GetLocal()
        {
            var locais = _localApp.GetAll().ToList();
            return locais;
        }

        [HttpGet]
        [Route("connection")]
        public IHttpActionResult Connection()
        {
            string myResult =" true";
            return Ok(myResult);
        }

        [HttpGet]
        [Route("usuarioid/{email}")]
        public int UsuarioId(string email)
        {
            return _usuarioApp.ObterIdPorEmail(email);
        }
    }
}

