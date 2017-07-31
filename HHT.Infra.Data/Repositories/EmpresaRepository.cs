using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Infra.CrossCutting.Helper;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace HHT.Infra.Data.Repositories
{
    public class EmpresaRepository : RepositoryBase<Empresa>, IEmpresaRepository
    {
        public Empresa ObterDocumento(int empresaId)
        {
            try
            {
                return db.Empresas
                            .Include("ArquivosEmpresa")
                            .Include("ArquivosEmpresa.DocumentosGeral")
                            .Where(c => c.EmpresaId == empresaId).FirstOrDefault();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void SalvarServicosSelecionados(Empresa empresa)
        {
            try
            {
                //get the id of the current recipe
                int id = empresa.EmpresaId;
                //load recipe with ingredients from the database
                var recipeItem = db.Empresas.Include("MappedServico").Single(r => r.EmpresaId == id);
                //apply the values that have changed
                db.Entry(recipeItem).CurrentValues.SetValues(empresa);
                //clear the ingredients to let the framework know they have to be processed
                recipeItem.MappedServico.Clear();
                //now reload the ingredients again, but from the list of selected ones as per model provided by the view
                foreach (int ingId in empresa.ServicosSelecionados)
                {
                    var servico = db.Servicos.Find(ingId);

                    MappedServico map = new MappedServico();
                    map.ServicoId = servico.ServicoId;
                    map.Nome = servico.Nome;

                    recipeItem.MappedServico.Add(map);
                }
                //finally, save changes as usual
                db.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        public void SalvarLocaisSelecionados(Empresa empresa)
        {
            try
            {
                //get the id of the current recipe
                int id = empresa.EmpresaId;
                //load recipe with ingredients from the database
                var recipeItem = db.Empresas.Include("MappedEmpresaLocal").Single(r => r.EmpresaId == id);
                //apply the values that have changed
                db.Entry(recipeItem).CurrentValues.SetValues(empresa);
                //clear the ingredients to let the framework know they have to be processed
                recipeItem.MappedEmpresaLocal.Clear();
                //now reload the ingredients again, but from the list of selected ones as per model provided by the view
                foreach (int ingId in empresa.LocaisSelecionados)
                {
                    var local = db.Locais.Find(ingId);

                    MappedEmpresaLocal map = new MappedEmpresaLocal();
                    map.Sigla = local.Sigla;
                    map.Nome = local.Nome;
                    map.LocalId = local.LocalId;

                    recipeItem.MappedEmpresaLocal.Add(map);

                }
                //finally, save changes as usual
                db.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public Empresa ObterServicosPorId(Empresa empresa)
        {
            try
            {
                return db.Empresas.Include("MappedServico").Single(s => s.EmpresaId == empresa.EmpresaId);
            }
            catch (System.Exception)
            {
                throw;
            }
        }



        public Empresa ObterLocaisPorId(Empresa empresa)
        {
            try
            {
                return db.Empresas.Include("MappedEmpresaLocal").Single(s => s.EmpresaId == empresa.EmpresaId);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private void UpdateEmpresaServicos(int[] selecionados, Empresa updateEmpresa)
        {
            try
            {
                if (selecionados == null)
                {
                    updateEmpresa.MappedServico = new List<MappedServico>();
                    return;
                }

                var selecionadosX = new HashSet<int>(selecionados);
                var usuarioServicos = new HashSet<int>(updateEmpresa.MappedServico.Select(m => m.MappedServicoId));

                foreach (var servico in db.MappedServico)
                {
                    if (selecionadosX.Contains(servico.MappedServicoId))
                    {
                        if (!usuarioServicos.Contains(servico.MappedServicoId))
                        {
                            updateEmpresa.MappedServico.Add(servico);
                        }
                    }
                    else
                    {
                        if (usuarioServicos.Contains(servico.MappedServicoId))
                        {
                            updateEmpresa.MappedServico.Remove(servico);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        private void UpdateEmpresaLocais(int[] selecionados, Empresa updateEmpresa)
        {
            try
            {
                if (selecionados == null)
                {
                    updateEmpresa.MappedEmpresaLocal = new List<MappedEmpresaLocal>();
                    return;
                }

                var selecionadosX = new HashSet<int>(selecionados);
                var usuariosLocais = new HashSet<int>(updateEmpresa.MappedEmpresaLocal.Select(m => m.MappedEmpresaLocalId));

                foreach (var locais in db.MappedEmpresaLocal)
                {
                    if (selecionadosX.Contains(locais.MappedEmpresaLocalId))
                    {
                        if (!usuariosLocais.Contains(locais.MappedEmpresaLocalId))
                        {
                            updateEmpresa.MappedEmpresaLocal.Add(locais);
                        }
                    }
                    else
                    {
                        if (usuariosLocais.Contains(locais.MappedEmpresaLocalId))
                        {
                            updateEmpresa.MappedEmpresaLocal.Remove(locais);
                        }
                    }
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void RemoverEspecifico(int empresaId)
        {
            try
            {
                var empresa = db.Empresas.Include("MappedServico").SingleOrDefault(a => a.EmpresaId == empresaId);

                if (empresa.ArquivosEmpresa.Count() == 0)
                {
                    foreach (MappedServico item in empresa.MappedServico.ToList())
                    {
                        empresa.MappedServico.Remove(item);
                        db.MappedServico.Remove(item);
                    }


                    if (empresa.ArquivosEmpresa.Count() == 0)
                    {
                        foreach (MappedEmpresaLocal item in empresa.MappedEmpresaLocal.ToList())
                        {
                            empresa.MappedEmpresaLocal.Remove(item);
                            db.MappedEmpresaLocal.Remove(item);
                        }


                    }
                    db.Empresas.Remove(empresa);
                    db.SaveChanges();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }


        public void RemoverLocalEspecifico(int empresaId)
        {
            try
            {
                var empresa = db.Empresas.Include("MappedEmpresaLocal").SingleOrDefault(a => a.EmpresaId == empresaId);

                if (empresa.ArquivosEmpresa.Count() == 0)
                {
                    foreach (MappedEmpresaLocal item in empresa.MappedEmpresaLocal.ToList())
                    {
                        empresa.MappedEmpresaLocal.Remove(item);
                        db.MappedEmpresaLocal.Remove(item);
                    }

                    db.Empresas.Remove(empresa);
                    db.SaveChanges();
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public Usuario ObterUsuarioVinculado(int empresaId)
        {
            return db.Usuarios.FirstOrDefault(u => u.EmpresaId == empresaId);
        }

        public Contratado ObterContratadoVinculado(int? empresaId, int? usuarioId)
        {
            if (empresaId != null)
            {
                return db.Contratados.FirstOrDefault(u => u.EmpresaId == empresaId);
            }

            return db.Contratados.FirstOrDefault(u => u.UsuarioId == usuarioId);
        }

        public DateTime? ObterMenorDataValidade(int empresaId)
        {
            try
            {
                var documentos = db.DocumentosGeral.Where(d => d.Associado.Nome == "Empresa").ToList();

                List<DateTime> datasAdicionadoVencimento = new List<DateTime>();

                var empresa = db.Empresas.Include("ArquivosEmpresa").Include("ArquivosEmpresa.DocumentosGeral").Where(e => e.EmpresaId == empresaId).FirstOrDefault();

                if (empresa.ArquivosEmpresa.Count() > 0)
                {
                    foreach (var item in empresa.ArquivosEmpresa)
                    {
                        var validadeMeses = documentos.Where(d => d.DocumentoGeralId == item.DocumentoGeralId).Select(s => s.Vencimento).FirstOrDefault();

                        var dataDeValidade = item.DataDocumento.AddMonths(Convert.ToInt32(validadeMeses));

                        datasAdicionadoVencimento.Add(dataDeValidade);
                    }

                    return datasAdicionadoVencimento.OrderBy(x => x).ElementAt(0);
                }

                return (DateTime?)null;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<Empresa> ObterPorLocal(int localId)
        {
            return db.Empresas.Include("MappedEmpresaLocal")
                 .Where(c => c.MappedEmpresaLocal.Where(z => z.LocalId == localId).FirstOrDefault().LocalId == localId).ToList();
        }
        
        public IEnumerable<Empresa> ObterPorPerfil(string role, string userName)
        {
            if (role.Equals(Enumerador.Perfil.Empresa.ToString()))
            {
                var usuario = db.Usuarios.Where(u => u.Email.Equals(userName)).FirstOrDefault();

                return db.Empresas.Where(e => e.EmpresaId.Equals(usuario.EmpresaId)).ToList();
            }

            return db.Empresas.ToList();
        }
    }
}
