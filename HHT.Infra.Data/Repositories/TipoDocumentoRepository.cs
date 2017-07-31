using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HHT.Infra.Data.Repositories
{
    public class TipoDocumentoRepository : RepositoryBase<TipoDocumento>, ITipoDocumentoRepository
    {
        public IEnumerable<TipoDocumento> ObterTiposDocumentoContratado()
        {
            var documentos = db.DocumentosGeral.Where(d => d.Associado.Nome == "Contratado").ToList();

            List<TipoDocumento> listTipoDocumento = new List<TipoDocumento>();

            foreach(var item in documentos)
            {
                TipoDocumento tipo = new TipoDocumento();
                tipo.TipoDocumentoId = item.TipoDocumento.TipoDocumentoId;
                tipo.Nome = item.TipoDocumento.Nome;
                tipo.DataCadastro = item.TipoDocumento.DataCadastro;

                listTipoDocumento.Add(tipo);
            }

            return listTipoDocumento.GroupBy(d => d.Nome).Select(d => d.FirstOrDefault());
        }
    }
}
