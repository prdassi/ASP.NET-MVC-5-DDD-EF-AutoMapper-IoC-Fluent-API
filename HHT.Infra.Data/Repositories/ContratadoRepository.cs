using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Infra.CrossCutting.Helper;
using MessagingToolkit.QRCode.Codec;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;

namespace HHT.Infra.Data.Repositories
{
    public class ContratadoRepository : RepositoryBase<Contratado>, IContratadoRepository
    {
        public Contratado ObterDocumentos(int contratadoId)
        {
            try
            {
                var result = db.Contratados
               .Include("Funcao")
               .Include("Empresa")
               .Include("Usuario")
               .Include("ArquivosContratado.DocumentoGeral")
               .Where(c => c.ContratadoId == contratadoId).FirstOrDefault();

                foreach (var item in result.ArquivosContratado)
                {

                    if (item.DocumentoGeral.Vencimento > 0)
                    {
                        item.DataDocumento = item.DataDocumento.Value.AddMonths(item.DocumentoGeral.Vencimento);
                    }
                    else
                    {
                        item.DataDocumento = (DateTime?)null;
                    }
                }

                return result;


            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public Contratado ObterPorRG(int localId, string rg)
        {
            try
            {
                var result = db.Contratados
                         .Include("Empresa")
                         .Include("Empresa.MappedEmpresaLocal")
                         .Where(c => c.RG == rg && (c.Empresa.MappedEmpresaLocal.Where(z => z.LocalId == localId).FirstOrDefault().LocalId == localId));

                return result.SingleOrDefault();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public IEnumerable<Contratado> ObterPorNome(int localId, string nome)
        {
            try
            {
                var result = db.Contratados
                             .Include("Empresa")
                             .Include("Empresa.MappedEmpresaLocal")
                             .Where(c => c.Nome.Contains(nome) && (c.Empresa.MappedEmpresaLocal.Where(z => z.LocalId == localId).FirstOrDefault().LocalId == localId));

                return result;

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public IEnumerable<Contratado> ObterPorLocal(int localId)
        {
            try
            {
                return db.Contratados
                         .Include("Empresa")
                            .Include("Empresa.MappedEmpresaLocal")
                             .Where(c => c.Empresa.MappedEmpresaLocal.Where(z => z.LocalId == localId).FirstOrDefault().LocalId == localId);
         


            }
            catch (System.Exception)
            {
                throw;
            }
        }


        public IEnumerable<Contratado> ObterPorEmpresa(int empresaId)
        {
            try
            {
                return db.Contratados.Where(c => c.EmpresaId == empresaId).ToList();
            }
            catch (System.Exception)
            {

                throw;
            }
        }

        public Identificacao Identificacao(int contratadoId)
        {
            var contratado = db.Contratados
                            .Include("Funcao")
                            .Include("Empresa")
                            .Include("Usuario")
                            .Include("ArquivosContratado.DocumentoGeral")
                            .Where(c => c.ContratadoId == contratadoId).FirstOrDefault();

            Identificacao ident = new Domain.Entities.Identificacao();

            ident.Documentos = new List<Identificacao.Documento>();
            ident.Locais = new List<Identificacao.LocalIntegracao>();

            int treinamentoId = db.TiposDocumento.Where(d => d.Nome.Equals("Treinamento")).Select(d => d.TipoDocumentoId).FirstOrDefault();
            int integracaoId = db.TiposDocumento.Where(d => d.Nome.Equals("Integração")).Select(d => d.TipoDocumentoId).FirstOrDefault();
            int documentoId = db.TiposDocumento.Where(d => d.Nome.Equals("Documento")).Select(d => d.TipoDocumentoId).FirstOrDefault();

            // Preencher minha lista com todos os treinamentos existentes da base
            foreach (var item in db.DocumentosGeral.Where(d => d.TipoDocumentoId == treinamentoId))
            {
                Identificacao.Documento doc = new Identificacao.Documento();
                doc.Nome = item.Nome;
                doc.Realizado = "Não Autorizado";

                ident.Documentos.Add(doc);
            }

            // Verifica qual treinamento foi realizado
            foreach (var item in ident.Documentos)
            {
                var documento = contratado.ArquivosContratado.Where(a => a.DocumentoGeral.Nome.Equals(item.Nome)).FirstOrDefault();

                if (documento != null)
                {
                    item.Realizado = documento.DataDocumento.Value.AddMonths(documento.DocumentoGeral.Vencimento).ToShortDateString();
                }
            }

            // Preencher minha lista com os locais
            foreach (var item in db.Locais.Where(l => l.Nome != "").ToList())
            {
                Identificacao.LocalIntegracao local = new Identificacao.LocalIntegracao();
                local.Sigla = item.Sigla;
                local.Realizado = "Não Possui";

                ident.Locais.Add(local);
            }

            // Verifica em qual local foi realizado a integracao
            foreach (var item in ident.Locais)
            {
                var integracao = contratado.ArquivosContratado.Where(a => a.TipoDocumentoId.Equals(integracaoId) && a.DocumentoGeral.Nome == "Local" && a.Local.Sigla == item.Sigla).FirstOrDefault();

                if (integracao != null)
                {
                    item.Realizado = integracao.DataDocumento.Value.ToShortDateString();
                }
            }

            // Integração Completa
            var integracaoCompleta = contratado.ArquivosContratado.Where(a => a.TipoDocumentoId == integracaoId && a.DocumentoGeral.Nome == "Completa").FirstOrDefault();
            if (integracaoCompleta != null)
            {
                ident.IntegracaoCompleta = integracaoCompleta.DataDocumento.Value.ToShortDateString();
            }

            // Atestado de Saúde Ocupacional
            var aso = contratado.ArquivosContratado.Where(a => a.TipoDocumentoId == documentoId && a.DocumentoGeral.Nome == "ASO").FirstOrDefault();
            if (aso != null)
            {
                ident.ASO = aso.DataDocumento.Value.ToShortDateString();
            }

            if (contratado.ValidadeDocumento != null)
            {
                ident.AcessoPermitido = contratado.ValidadeDocumento.Value.ToShortDateString();
            }

            ident.AprovadoPor = contratado.Usuario.Nome;
            ident.Data = DateTime.Now.Day + " de " + FormatarAnoMesDia.MesPorExtenso(DateTime.Now.Month) + ", " + DateTime.Now.Year;
            ident.RG = contratado.RG;
            ident.Empresa = contratado.Empresa.Nome;
            ident.Colaborador = contratado.Nome;
            ident.Funcao = contratado.Funcao.Nome;
            ident.QRCode = ImagemQRCode(ident);

            return ident;
        }

        private string ImagemQRCode(Identificacao identificador)
        {
            QRCodeEncoder qrCodecEncoder = new QRCodeEncoder();
            qrCodecEncoder.QRCodeBackgroundColor = System.Drawing.Color.White;
            qrCodecEncoder.QRCodeForegroundColor = System.Drawing.Color.Black;
            qrCodecEncoder.CharacterSet = "UTF-8";
            qrCodecEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qrCodecEncoder.QRCodeScale = 6;
            qrCodecEncoder.QRCodeVersion = 0;
            qrCodecEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.Q;
            int qrcodeHeight = 0;
            var qrcodeWitdh = 0;

            //Criar diretório
            string caminhoPasta = ConfigurationManager.AppSettings["PastaQRCode"];
            var pastaPath = HttpContext.Current.Server.MapPath(caminhoPasta);
            if (!Directory.Exists(pastaPath))
            {
                Directory.CreateDirectory(pastaPath);
            }

            int.TryParse(ConfigurationManager.AppSettings["AlturaQRCode"], out qrcodeHeight);
            int.TryParse(ConfigurationManager.AppSettings["LarguraQRCode"], out qrcodeWitdh);

            var imageQRCode = new Bitmap(qrcodeHeight, qrcodeWitdh);

            string dados = identificador.Colaborador + " | " + identificador.RG + "|" + identificador.Empresa;
            imageQRCode = qrCodecEncoder.Encode(dados);

            string nomeArquivo = GeradorString.RandomAlfanumerico(identificador.RG);
            string caminho = ConfigurationManager.AppSettings["PastaQRCode"] + nomeArquivo + ".png";
            imageQRCode.Save(HttpContext.Current.Server.MapPath(caminho));

            return nomeArquivo + ".png";
        }

        public void ExcluirQRCode(string nome)
        {
            string caminho = ConfigurationManager.AppSettings["PastaQRCode"] + nome;

            if (File.Exists(caminho))
            {
                File.Delete(HttpContext.Current.Server.MapPath(caminho));
            }
        }

        public IEnumerable<Contratado> ObterPorPerfil(string role, string userName)
        {
            if (role.Equals(Enumerador.Perfil.Empresa.ToString()))
            {
                var usuario = db.Usuarios.Where(u => u.Email.Equals(userName)).FirstOrDefault();

                return db.Contratados.Where(c => c.EmpresaId == usuario.EmpresaId).ToList();
            }

            return db.Contratados.ToList();
        }
    }
}
