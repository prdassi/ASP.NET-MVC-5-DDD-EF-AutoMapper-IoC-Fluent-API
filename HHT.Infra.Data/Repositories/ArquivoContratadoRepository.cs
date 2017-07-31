using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using System.Web;
using System.Linq;
using System.IO;
using System.Configuration;
using System.Web.Configuration;
using System;
using System.Web.UI.WebControls;
using System.Data.Entity;
using HHT.Infra.CrossCutting.Helper;

namespace HHT.Infra.Data.Repositories
{
    public class ArquivoContratadoRepository : RepositoryBase<ArquivoContratado>, IArquivoContratadoRepository
    {
        public void AdicionaDocumento(ArquivoContratado arquivoContratado, int contratadoId, HttpPostedFileBase upload)
        {
            try
            {
                //Arquivo
                SalvarArquivo(arquivoContratado, upload);

                db.Contratados.FirstOrDefault(x => x.ContratadoId == contratadoId).ArquivosContratado.Add(arquivoContratado);

                db.SaveChanges();

                AjustarVencimentoCurso(arquivoContratado, contratadoId);
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public void EditarDocumento(ArquivoContratado arquivoContratado, HttpPostedFileBase upload, int contratadoId)
        {
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    ExcluirArquivo(arquivoContratado);

                    ArquivoContratado ac = new ArquivoContratado();

                    ac = SalvarArquivo(arquivoContratado, upload);

                    db.Entry(ac).State = EntityState.Modified;

                    db.SaveChanges();
                }
                else
                {
                    db.Entry(arquivoContratado).State = EntityState.Modified;
                    db.SaveChanges();
                }

                AjustarVencimentoCurso(arquivoContratado, contratadoId);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ExcluirArquivo(ArquivoContratado arquivoContratado)
        {
            try
            {
                var file = db.ArquivosContratado.FirstOrDefault(a => a.ArquivoContratadoId == arquivoContratado.ArquivoContratadoId);

                Configuration root = WebConfigurationManager.OpenWebConfiguration(null);

                string caminho = ConfigurationManager.AppSettings["PastaUploadContratado"];

                var path = Path.Combine(caminho, file.Nome + file.Tipo);

                var pastaPath = HttpContext.Current.Server.MapPath(path);

                File.Delete(pastaPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ArquivoContratado SalvarArquivo(ArquivoContratado arquivoContratado, HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    Configuration root = WebConfigurationManager.OpenWebConfiguration(null);

                    string caminho = ConfigurationManager.AppSettings["PastaUploadContratado"];
                    var fileName = Path.GetFileName(file.FileName);
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                    var nomeGerado = fileNameWithoutExtension + '_' + GeradorString.RandomAlfanumerico(fileNameWithoutExtension);

                    arquivoContratado.Nome = nomeGerado;
                    arquivoContratado.Tipo = Path.GetExtension(fileName);
                    arquivoContratado.Tamanho = file.ContentLength.ToString();
                    arquivoContratado.DataDocumento = arquivoContratado.DataDocumento;
                    arquivoContratado.DocumentoGeralId = arquivoContratado.DocumentoGeralId;

                    var path = Path.Combine(caminho, fileName);

                    var pastaPath = HttpContext.Current.Server.MapPath(caminho);

                    if (!Directory.Exists(pastaPath))
                    {
                        Directory.CreateDirectory(pastaPath);
                    }

                    FileUpload fileUpload = new FileUpload();

                    file.SaveAs(pastaPath + fileName);

                    //Renomear arquivo igual ao banco
                    Directory.Move(pastaPath + fileName, pastaPath + nomeGerado + arquivoContratado.Tipo);

                    return arquivoContratado;
                }
                else
                {
                    //Response.Write("Por favor, selecione um arquivo para fazer o upload.");
                }

                return arquivoContratado;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Exclui contratado, serviços, ponto e arquivos associados.
        public void ExcluirContratadoCompleto(int arquivoContratadoId, int contratadoId)
        {
            try
            {
                var contratado = db.Contratados.SingleOrDefault(a => a.ContratadoId == contratadoId);

                if (contratado.ArquivosContratado.Count() > 0)
                {
                    foreach (ArquivoContratado item in contratado.ArquivosContratado.ToList())
                    {
                        contratado.ArquivosContratado.Remove(item);
                        db.ArquivosContratado.Remove(item);

                        ExcluirArquivo(item);
                    }

                    db.Contratados.Remove(contratado);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void ExcluirDocumento(int arquivoContradoId, int contratadoId)
        {
            try
            {
                var arquivo = db.ArquivosContratado.FirstOrDefault(e => e.ArquivoContratadoId == arquivoContradoId);

                if (arquivo != null)
                {
                    db.Contratados.FirstOrDefault(x => x.ContratadoId == contratadoId).ArquivosContratado.Remove(arquivo);
                    db.ArquivosContratado.Remove(arquivo);

                    ExcluirArquivo(arquivo);

                    db.SaveChanges();

                    var arquivocontratado = db.ArquivosContratado.Where(a => a.ArquivoContratadoId == arquivoContradoId).FirstOrDefault();
                    AjustarVencimentoCurso(arquivocontratado, contratadoId);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ArquivoContratado ObterArquivoPorDocumento(int documentoId)
        {
            return db.ArquivosContratado.Where(a => a.DocumentoGeralId == documentoId).FirstOrDefault();
        }        

        public void AjustarVencimentoCurso(ArquivoContratado arquivoContratado, int contratadoId)
        {
            try
            {
                var arquivos = db.Contratados
                            .Include("Funcao")
                            .Include("Empresa")
                            .Include("Usuario")
                            .Include("ArquivosContratado.DocumentoGeral")
                            .Where(c => c.ContratadoId == contratadoId)
                            .Select(c => c.ArquivosContratado).FirstOrDefault();

                Contratado contratado = new Contratado();

                int validadeEmMeses = 0;

                contratado = db.Contratados.Where(c => c.ContratadoId == contratadoId).FirstOrDefault();

                Contratado alterarContratado = new Contratado();
                alterarContratado.ContratadoId = contratado.ContratadoId;
                alterarContratado.Nome = contratado.Nome;
                alterarContratado.RG = contratado.RG;
                alterarContratado.Ativo = contratado.Ativo;
                alterarContratado.DataAdmissao = contratado.DataAdmissao;
                alterarContratado.Restricao = contratado.Restricao;

                var id = 0;
                DateTime? data = null;

                foreach (var item in arquivos)
                {
                    if (item.DocumentoGeral == null)
                    {
                        var doc = db.DocumentosGeral.Where(d => d.DocumentoGeralId == item.DocumentoGeralId).FirstOrDefault();
                        item.DocumentoGeral.Vencimento = doc.Vencimento;
                    }

                    var vencimento = item.DataDocumento.Value.AddMonths(item.DocumentoGeral.Vencimento);

                    if (data == null)
                    {
                        id = item.ArquivoContratadoId;
                        data = vencimento;
                    }
                    else
                    {
                        if (data.Value.CompareTo(vencimento) > 0)
                        {
                            id = item.ArquivoContratadoId;
                            data = vencimento;
                        }
                    }
                }

                var menorVencimento = arquivos.Where(a => a.ArquivoContratadoId == id).FirstOrDefault();

                if (menorVencimento != null)
                {
                    if (menorVencimento.DocumentoGeral == null)
                    {
                        validadeEmMeses = db.DocumentosGeral.Where(d => d.DocumentoGeralId == menorVencimento.DocumentoGeralId).Select(d => d.Vencimento).FirstOrDefault();
                    }
                    else
                    {
                        validadeEmMeses = menorVencimento.DocumentoGeral.Vencimento;
                    }
                    if (validadeEmMeses > 0)
                    {
                        alterarContratado.ValidadeDocumento = menorVencimento.DataDocumento.Value.AddMonths(validadeEmMeses);//Data de vencimento já calculado os mêses
                    }
                }
                alterarContratado.Justificativa = contratado.Justificativa;
                alterarContratado.DataCadastro = contratado.DataCadastro;
                alterarContratado.FuncaoId = contratado.FuncaoId;
                alterarContratado.EmpresaId = contratado.EmpresaId;
                alterarContratado.UsuarioId = contratado.UsuarioId;

                db.Entry(contratado).CurrentValues.SetValues(alterarContratado);

                db.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
