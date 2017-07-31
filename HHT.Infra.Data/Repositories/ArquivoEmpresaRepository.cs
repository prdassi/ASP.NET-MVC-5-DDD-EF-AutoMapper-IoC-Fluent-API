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
    public class ArquivoEmpresaRepository : RepositoryBase<ArquivoEmpresa>, IArquivoEmpresaRepository
    {
        public void AdicionaDocumento(ArquivoEmpresa arquivoEmpresa, int empresaId, HttpPostedFileBase upload)
        {

            //Arquivo
            SalvarArquivo(arquivoEmpresa, upload);

            db.Empresas.FirstOrDefault(x => x.EmpresaId == empresaId).ArquivosEmpresa.Add(arquivoEmpresa);
            db.SaveChanges();
        }

        public void EditarDocumento(ArquivoEmpresa arquivoEmpresa, HttpPostedFileBase upload)
        {
            try
            {
                if (upload != null && upload.ContentLength > 0)
                {
                    ExcluirArquivo(arquivoEmpresa);

                    ArquivoEmpresa fd = new ArquivoEmpresa();

                    fd = SalvarArquivo(arquivoEmpresa, upload);

                    //db.Empresas.FirstOrDefault(x => x.EmpresaId == arquivoEmpresa.Empresas.FirstOrDefault().EmpresaId).ArquivosEmpresa.Add(arquivoEmpresa);

                    db.Entry(fd).State = EntityState.Modified;

                    db.SaveChanges();

                }
                else
                {
                    //var arquivo = db.ArquivosEmpresa.FirstOrDefault(x => x.ArquivoEmpresaId == arquivoEmpresa.ArquivoEmpresaId);

                    db.Entry(arquivoEmpresa).State = EntityState.Modified;
                    db.SaveChanges();

                    //arquivo.ArquivoEmpresaId = arquivoEmpresa.ArquivoEmpresaId;
                    //arquivo.DataCadastro = DateTime
                    //arquivo.DataDocumento



                    //db.SaveChanges();
                }

                
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void ExcluirArquivo(ArquivoEmpresa arquivoEmpresa)
        {
            try
            {
                    var file = db.ArquivosEmpresa.FirstOrDefault(a => a.ArquivoEmpresaId == arquivoEmpresa.ArquivoEmpresaId);

                    Configuration root = WebConfigurationManager.OpenWebConfiguration(null);

                    string caminho = ConfigurationManager.AppSettings["PastaUploadEmpresa"];                  

                    var path = Path.Combine(caminho, file.Nome + file.Tipo);

                    var pastaPath = HttpContext.Current.Server.MapPath(path);

                    File.Delete(pastaPath);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private ArquivoEmpresa SalvarArquivo(ArquivoEmpresa arquivoEmpresa, HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    Configuration root = WebConfigurationManager.OpenWebConfiguration(null);

                    string caminho = ConfigurationManager.AppSettings["PastaUploadEmpresa"];
                    var fileName = Path.GetFileName(file.FileName);
                    var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);

                    var nomeGerado = fileNameWithoutExtension + '_' + GeradorString.RandomAlfanumerico(fileNameWithoutExtension);

                    arquivoEmpresa.Nome = nomeGerado;
                    arquivoEmpresa.Tipo = Path.GetExtension(fileName);
                    arquivoEmpresa.Tamanho = file.ContentLength.ToString();
                    arquivoEmpresa.DataDocumento = arquivoEmpresa.DataDocumento;
                    arquivoEmpresa.DocumentoGeralId = arquivoEmpresa.DocumentoGeralId;

                    var path = Path.Combine(caminho, fileName);

                    var pastaPath = HttpContext.Current.Server.MapPath(caminho);

                    if(!Directory.Exists(pastaPath))
                    {
                        Directory.CreateDirectory(pastaPath);
                    }

                    FileUpload fileUpload = new FileUpload();

                    file.SaveAs(pastaPath + fileName);

                    //Renomear arquivo igual ao banco
                    Directory.Move(pastaPath + fileName, pastaPath + nomeGerado+ arquivoEmpresa.Tipo);

                    return arquivoEmpresa;
                }
                else
                {
                    //Response.Write("Por favor, selecione um arquivo para fazer o upload.");
                }

                return arquivoEmpresa;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Exclui empresa, serviços e arquivos associados.
        public void ExcluirEmpresaCompleta(int arquivoEmpresaId, int empresaId)
        {
            try
            {
                var empresa = db.Empresas.Include("MappedServico").SingleOrDefault(a => a.EmpresaId == empresaId);

                if (empresa.ArquivosEmpresa.Count() > 0)
                {
                    foreach (MappedServico item in empresa.MappedServico.ToList())
                    {
                        empresa.MappedServico.Remove(item);
                        db.MappedServico.Remove(item);
                    }

                    foreach (ArquivoEmpresa item in empresa.ArquivosEmpresa.ToList())
                    {
                        empresa.ArquivosEmpresa.Remove(item);
                        db.ArquivosEmpresa.Remove(item);

                        ExcluirArquivo(item);
                    }

                    db.Empresas.Remove(empresa);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        public void ExcluirDocumento(int arquivoEmpresaId, int empresaId)
        {
            try
            {
                var arquivo = db.ArquivosEmpresa.FirstOrDefault(e => e.ArquivoEmpresaId == arquivoEmpresaId);

                if (arquivo != null)
                {
                    db.Empresas.FirstOrDefault(x => x.EmpresaId == empresaId).ArquivosEmpresa.Remove(arquivo);
                    db.ArquivosEmpresa.Remove(arquivo);

                    ExcluirArquivo(arquivo);

                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ArquivoEmpresa ObterArquivoPorDocumento(int documentoId)
        {
            return db.ArquivosEmpresa.Where(a => a.DocumentoGeralId == documentoId).FirstOrDefault();
        }        
    }
}
