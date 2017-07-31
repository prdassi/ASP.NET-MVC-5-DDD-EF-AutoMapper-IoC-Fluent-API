using AutoMapper;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Infra.CrossCutting.Helper;
using HHT.Services.Mesnsagem;
using HHT.UI.ViewModels;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace HHT.UI.Controllers
{
    [Authorize]
    public class ConsolidacaoController : Controller
    {
        private readonly ILocalAppService _localApp;
        private readonly IContratadoAppService _contratadoApp;
        private readonly IPontoAppService _pontoApp;
        private readonly IEmpresaAppService _empresaApp;

        private readonly IConsolidacaoAppService _consolidacaoApp;

        public ConsolidacaoController(ILocalAppService localApp, IContratadoAppService contratadoApp, IPontoAppService pontoApp,
                                               IEmpresaAppService empresaApp, IConsolidacaoAppService consolidacaoApp)
        {
            _localApp = localApp;
            _contratadoApp = contratadoApp;
            _pontoApp = pontoApp;
            _empresaApp = empresaApp;
            _consolidacaoApp = consolidacaoApp;
        }


        // GET: Consolidacao
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

            var consolidacaoViewModel = Mapper.Map<IEnumerable<Consolidacao>, IEnumerable<ConsolidacaoViewModel>>(_consolidacaoApp.ObterResultados(localId, empresaId, ano, mes));

            return View("Index", consolidacaoViewModel);
        }

        public ActionResult PDF(int localId, int empresaId, int ano, int mes)
        {
            try
            {
                var consolidacaoViewModel = Mapper.Map<IEnumerable<Consolidacao>, IEnumerable<ConsolidacaoViewModel>>(_consolidacaoApp.ObterResultados(localId, empresaId, ano, mes));

                HtmlForm form = new HtmlForm();
                Document document = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);

                string relatorio = "<div class='col-sm-12'>" +
                        "<div class='panel panel-default'>" +
                          "<div class='panel-heading panel-heading-divider'>Relatório consolidado</div>" +
                            "<div class='panel-body' id='panelBody'>" +
                                "<table id = 'datatable' class='table table-striped table-hover table-fw-widget'>" +
                                    "<thead>" +
                                        "<tr>" +
                                            "<th rowspan = '1'></th>" +
                                            "<th colspan = '1'></th>";
                var meses = consolidacaoViewModel.Distinct().FirstOrDefault().Meses.GroupBy(x => x.Nome).Select(y => y.First()).ToList();

                foreach (var mesRelatorio in meses)
                {
                    relatorio = relatorio + "<th class='mes' colspan='2'>" + mesRelatorio.Nome + "</th>";

                }


                relatorio = relatorio + "</tr>" +
                "<tr>" +
                    "<th>Empresa</th>" +
                    "<th>Gestor</th>";
                foreach (var espec in consolidacaoViewModel)
                {
                    foreach (var x in espec.Meses)
                    {
                        relatorio = relatorio + "<th style='background-color: #76b4d0'>Nº Funcionário</th>" +
                                                            "<th style='background-color: #a6cfe4'>HHT</th>";

                    }
                }
                relatorio = relatorio + "</tr>" +
                                    "</thead>" +
                                    "<tbody>";
                foreach (var item in consolidacaoViewModel)
                {
                    relatorio = relatorio + "<tr class='odd gradeX'>" +
                                                "<td>" + item.Empresa + "</td>" +
                                                "<td>" + item.Gestor + "</td>";
                    foreach (var itens in item.Meses)
                    {
                        relatorio = relatorio + "<td class='text-center'>" + itens.NumeroFuncionario + "</td>" +
                                                            "<td class='text-center'>" + itens.HHT + "</td>";
                    }
                    relatorio = relatorio + "</tr>";
                }
                relatorio = relatorio + "</tbody>" +
                                "</table>" +
                            "</div>" +
                        "</div>" +
                    "</div>";

                Byte[] bytes;

                //Boilerplate iTextSharp setup here
                //Create a stream that we can write to, in this case a MemoryStream
                using (var ms = new MemoryStream())
                {
                    //Create an iTextSharp Document which is an abstraction of a PDF but **NOT** a PDF
                    using (var doc = new Document())
                    {
                        //Create a writer that's bound to our PDF abstraction and our stream
                        using (var writer = PdfWriter.GetInstance(doc, ms))
                        {
                            //Open the document for writing
                            doc.Open();

                            //Our sample HTML and CSS
                            // var example_html = @"<p>This <em>is </em><span class=""headline"" style=""text-decoration: underline;"">some</span> <strong>sample <em> text</em></strong><span style=""color: red;"">!!!</span></p>";
                            var example_css = @"th.mes { 
                                                background-color: #8bc63e;
                                                text-align:center;
                                                }

                                                .table {
                                                    width: 100%;
                                                    max-width: 100%;
                                                    margin-bottom: 18px;
                                                font-family: Roboto,Arial,sans-serif;
                                                    font-size: 13px;
                                                    line-height: 1.42857143;
                                                    color: #404040;
                                                }

                                                    .table > tbody > tr > td, .table > tbody > tr > th, .table > tfoot > tr > td, .table > tfoot > tr > th, .table > thead > tr > td, .table > thead > tr > th {
                                                        padding: 6px 10px;
                                                        line-height: 1.42857143;
                                                        vertical-align: top;
                                                        border-top: 1px solid #ddd;
                                                    }

                                                    .table > tbody > tr > td .icon-table {
                                                        float: left;
                                                        margin-right: 15px;
                                                    }

                                                    .table > thead > tr > th {
                                                        vertical-align: bottom;
                                                        border-bottom: 2px solid #ddd;
                                                    }

                                                    .table > caption + thead > tr:first-child > td, .table > caption + thead > tr:first-child > th, .table > colgroup + thead > tr:first-child > td, .table > colgroup + thead > tr:first-child > th, .table > thead:first-child > tr:first-child > td, .table > thead:first-child > tr:first-child > th {
                                                        border-top: 0;
                                                    }

                                                    .table > tbody + tbody {
                                                        border-top: 2px solid #ddd;
                                                    }

                                                    .table .table {
                                                        background-color: #EEE;
                                                    }


                                                #panelBody {
                                                    white-space: nowrap;
                                                }
                                                    #panelBody .dataTables_scrollHeadInner > table, #panelBody .dataTables_scrollBody > table {
                                                        width: 1140px !important;
                                                        white-space: nowrap;
                                                    }

                                                ";

                            //In order to read CSS as a string we need to switch to a different constructor
                            //that takes Streams instead of TextReaders.
                            //Below we convert the strings into UTF8 byte array and wrap those in MemoryStreams
                            using (var msCss = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(example_css)))
                            {
                                using (var msHtml = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(relatorio)))
                                {

                                    //Parse the HTML
                                    iTextSharp.tool.xml.XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, msHtml, msCss);
                                }
                            }

                            doc.Close();
                        }
                    }

                    //After all of the PDF "stuff" above is done and closed but **before** we
                    //close the MemoryStream, grab all of the active bytes from the stream
                    bytes = ms.ToArray();

                    //Now we just need to do something with those bytes.
                    //Here I'm writing them to disk but if you were in ASP.Net you might Response.BinaryWrite() them.
                    //You could also write the bytes to a database in a varbinary() column (but please don't) or you
                    //could pass them to another function for further PDF processing.
                    // var testFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Relatorio_Consolidado.pdf");
                    //System.IO.File.WriteAllBytes(testFile, bytes);

                    Response.ClearHeaders();
                    Response.ClearContent();
                    Response.AddHeader("Content-Type", "application/pdf");
                    Response.AddHeader("Content-Disposition", "attachment; filename=Relatorio_Consolidado" + ".pdf");
                    Response.ContentType = "application/octet-stream";
                    Response.BinaryWrite(bytes);

                    Response.Flush();
                    Response.End();
                    System.Web.HttpContext.Current.ApplicationInstance.CompleteRequest();

                }
            }
            catch (ThreadAbortException)
            {
                return RedirectToAction("Index").Mensagem("Por favor, tente gerar novamente o relatório", "Erro");
            }

            return null;
        }

        public ActionResult Excel(int localId, int empresaId, int ano, int mes)
        {
            var consolidacaoViewModel = Mapper.Map<IEnumerable<Consolidacao>, IEnumerable<ConsolidacaoViewModel>>(_consolidacaoApp.ObterResultados(localId, empresaId, ano, mes));

            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();

            sb.Append("<div class='col-sm-12'>");
            sb.Append("<div class='panel panel-default'>");
            sb.Append("<div class='panel-heading panel-heading-divider'>Relatório consolidado</div>");
            sb.Append("<div class='panel-body' style='white-space: nowrap'>");
            sb.Append("<table id='datatable' class='table table-striped table-hover table-fw-widget'>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th style='vertical-align: bottom; border-bottom: 2px solid #ddd;' rowspan='1'></th>");
            sb.Append("<th style='vertical-align: bottom; border-bottom: 2px solid #ddd;' colspan='1'></th>");

            var meses = consolidacaoViewModel.Distinct().FirstOrDefault().Meses.GroupBy(x => x.Nome).Select(y => y.First()).ToList();

            foreach (var mesRelatorio in meses)
            {
                sb.Append("<th style='padding: 6px 10px;line-height: 1.42857143;vertical-align: top;vertical-align: bottom; border-bottom: 2px solid #ddd;background-color: #8bc63e; text-align:center' colspan='2'>" + mesRelatorio.Nome + "</th>");
            }
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<th>Empresa</th>");
            sb.Append("<th>Gestor</th>");
            foreach (var espec in consolidacaoViewModel)
            {
                foreach (var x in espec.Meses)
                {
                    sb.Append("<th style='background-color: #76b4d0'>Nº Funcionário</th>");
                    sb.Append("<th style='background-color: #a6cfe4'>HHT</th>");
                }
            }
            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");
            foreach (var item in consolidacaoViewModel)
            {
                sb.Append("<tr class='odd gradeX'>");
                sb.Append("<td>" + item.Empresa + "</td>");
                sb.Append("<td>" + item.Gestor + "</td>");
                foreach (var itens in item.Meses)
                {
                    sb.Append("<td class='text-center'>" + itens.NumeroFuncionario + "</td>");
                    sb.Append("<td class='text-center'>" + itens.HHT + "</td>");
                }
                sb.Append("</tr>");
            }
            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");
            sb.Append("</div>");
            sb.Append("</div>");

            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Relatorio_Consolidado.xls");
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252");
            Response.Charset = "ISO-8859-1";
            Response.ContentType = "application/vnd.ms-excel";
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();

            return null;
        }
    }
}