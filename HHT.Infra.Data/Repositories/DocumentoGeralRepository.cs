using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HHT.Infra.Data.Repositories
{
    public class DocumentoGeralRepository : RepositoryBase<DocumentoGeral>, IDocumentoGeralRepository
    {
        public IEnumerable<DocumentoGeral> ObterDocumentosEmpresa()
        {
            var documentos = db.DocumentosGeral.Where(d => d.Associado.Nome == "Empresa").ToList();

            return documentos.GroupBy(g => g.Nome).Select(s => s.FirstOrDefault()).ToList();
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosContratado()
        {
            var documentos = db.DocumentosGeral.Where(d => d.Associado.Nome == "Contratado").ToList();

            return documentos.GroupBy(g => g.Nome).Select(s => s.FirstOrDefault()).ToList();
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosRestantes(int empresaId)
        {
            var documentos = db.DocumentosGeral.Where(d => d.AssociadoId == 1 && d.TipoDocumentoId == 1).ToList();

            var empresa = db.Empresas
                           .Include("ArquivosEmpresa")
                           .Include("ArquivosEmpresa.DocumentosGeral")
                           .Where(c => c.EmpresaId == empresaId).FirstOrDefault();

            if (documentos.Count() > 0)
            {
                foreach (var item in empresa.ArquivosEmpresa)
                {
                    documentos.Remove(item.DocumentosGeral);
                }
            }

            return documentos;
        }

        public IEnumerable<DocumentoGeral> ObterDocumentosRestantesIncluido(int empresaId, int arquivoEmpresaId)
        {
            var documentos = db.DocumentosGeral.Where(d => d.AssociadoId == 1 && d.TipoDocumentoId == 1).ToList();

            var arquivoEmpresa = db.ArquivosEmpresa.Where(a => a.ArquivoEmpresaId == arquivoEmpresaId).FirstOrDefault();

            var empresa = db.Empresas
                           .Include("ArquivosEmpresa")
                           .Include("ArquivosEmpresa.DocumentosGeral")
                           .Where(c => c.EmpresaId == empresaId).FirstOrDefault();


            if (documentos.Count() > 0)
            {
                foreach (var item in empresa.ArquivosEmpresa)
                {
                    if (item.DocumentoGeralId != arquivoEmpresa.DocumentoGeralId)
                    {
                        documentos.Remove(item.DocumentosGeral);
                    }
                }
            }

            return documentos;
        }
    }
}
