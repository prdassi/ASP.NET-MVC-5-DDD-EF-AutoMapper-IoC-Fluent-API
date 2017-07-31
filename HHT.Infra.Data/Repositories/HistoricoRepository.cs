using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HHT.Infra.Data.Repositories
{
    public class HistoricoRepository : RepositoryBase<Historico>, IHistoricoRepository
    {
        public void Adicionar(Historico historico)
        {
            try
            {
                Historico h = new Historico();

                h.Data = historico.Data;
                h.Descricao = historico.Descricao;
                h.Contradado = db.Contratados.Where(c => c.ContratadoId.Equals(historico.Contradado.ContratadoId)).FirstOrDefault();
                h.UsuarioId = historico.UsuarioId;

                db.Entry(h.Contradado).State = EntityState.Modified;

                db.Historicos.Add(h);
                db.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }            
        }

        public IEnumerable<Historico> ObterPorId(int contratadoId)
        {
            try
            {
                var historico = db.Historicos.Where(h => h.ContratadoId.Equals(contratadoId)).OrderBy(o => o.Data.Hour);
                return historico;
            }
            catch (System.Exception)
            {
                throw;
            }                        
        }

        public void RemoveById(int contratadoId)
        {
            try
            {
                db.Historicos.RemoveRange(db.Historicos.Where(x => x.ContratadoId == contratadoId));
                db.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
