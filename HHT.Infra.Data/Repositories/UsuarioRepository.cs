using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Infra.CrossCutting.Helper;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace HHT.Infra.Data.Repositories
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public void SalvarLocaisSelecionados(Usuario usuario)
        {
            //get the id of the current recipe
            int id = usuario.UsuarioId;
            //load recipe with ingredients from the database
            var recipeItem = db.Usuarios.Include(r => r.MappedLocal).Single(r => r.UsuarioId == id);
            //apply the values that have changed
            db.Entry(recipeItem).CurrentValues.SetValues(usuario);
            //clear the ingredients to let the framework know they have to be processed
            recipeItem.MappedLocal.Clear();
            //now reload the ingredients again, but from the list of selected ones as per model provided by the view
            foreach (int ingId in usuario.LocaisSelecionados)
            {
                var local = db.Locais.Find(ingId);

                MappedLocal map = new MappedLocal();
                map.Sigla = local.Sigla;
                map.Nome = local.Nome;
                map.LocalId = local.LocalId;

                recipeItem.MappedLocal.Add(map);
            }
            //finally, save changes as usual
            db.SaveChanges();
        }

        public Usuario ObterLocaisPorId(Usuario usuario)
        {
            //return db.Usuarios.Include("MappedLocal").Where(i => i.UsuarioId == usuario.UsuarioId).Single();
            return db.Usuarios.Include("MappedLocal").Single(s => s.UsuarioId == usuario.UsuarioId);
        }

        public Usuario Login(string email, string senha)
        {
            var usuario = db.Usuarios.Where(u => u.Email.Equals(email)).FirstOrDefault();

            if (usuario != null)
            {
                if (Segurancao.VerifyMd5Hash(senha, usuario.Senha))
                {
                    return usuario;
                }
            }

            return null;
        }

        private void UpdateUsuarioLocais(int[] selecionados, Usuario updateUsuario)
        {
            if (selecionados == null)
            {
                updateUsuario.MappedLocal = new List<MappedLocal>();
                return;
            }

            var selecionadosX = new HashSet<int>(selecionados);
            var usuarioLocais = new HashSet<int>(updateUsuario.MappedLocal.Select(m => m.MappedLocalId));

            foreach (var local in db.MappedLocal)
            {
                if (selecionadosX.Contains(local.MappedLocalId))
                {
                    if (!usuarioLocais.Contains(local.MappedLocalId))
                    {
                        updateUsuario.MappedLocal.Add(local);
                    }
                }
                else
                {
                    if (usuarioLocais.Contains(local.MappedLocalId))
                    {
                        updateUsuario.MappedLocal.Remove(local);
                    }
                }
            }
        }

        public void RemoveEspecifico(int usuarioId)
        {
            var usuario = db.Usuarios.Include(a => a.MappedLocal).SingleOrDefault(a => a.UsuarioId == usuarioId);

            foreach (MappedLocal item in usuario.MappedLocal.ToList())
            {
                usuario.MappedLocal.Remove(item);
                db.MappedLocal.Remove(item);
            }

            db.Usuarios.Remove(usuario);
            db.SaveChanges();
        }

        public Usuario ObterUsuarioLogado(string userName)
        {
            return db.Usuarios.Where(u => u.Email.Equals(userName)).FirstOrDefault();
        }

        public int ObterIdPorEmail(string email)
        {
            return db.Usuarios.Where(u => u.Email.Equals(email)).Select(s => s.UsuarioId).FirstOrDefault();
        }
    }
}
