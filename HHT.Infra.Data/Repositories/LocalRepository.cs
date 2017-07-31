using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Infra.CrossCutting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HHT.Infra.Data.Repositories
{
    public class LocalRepository : RepositoryBase<Local>, ILocalRepository
    {
        public int GetNull()
        {
            return db.Locais.Where(l => String.IsNullOrEmpty(l.Nome) && String.IsNullOrEmpty(l.Sigla)).Select(l => l.LocalId).FirstOrDefault();
        }
        
        public int[] ObterLocaisEmpresaSeleciponados(Empresa empresa)
        {
            int[] myIndices = new int[empresa.MappedEmpresaLocal.Count];

            List<int> list = new List<int>();

            foreach (var selecao in empresa.MappedEmpresaLocal)
            {
                list.Add(selecao.LocalId);
            }

            return list.Cast<int>().ToArray();
        }

        public IEnumerable<Local> ObterPorPerfil(string role, string userName)
        {
            if (role == Enumerador.Perfil.Porteiro.ToString())
            {
                var usuarios = db.Usuarios.Where(u => u.Email.Equals(userName)).FirstOrDefault();

                List<Local> list = new List<Local>();
                foreach (var item in usuarios.MappedLocal)
                {
                    Local local = new Local();
                    local.DataCadastro = item.DataCadastro;
                    local.LocalId = item.LocalId;
                    local.Nome = item.Nome;
                    local.Sigla = item.Sigla;

                    list.Add(local);
                }

                return list.ToList(); 
            }

            var allItens = db.Locais.ToList();
            allItens.RemoveAt(0);

            return allItens.ToList();            
        }

        public List<Local> ObterPorEmail(string email)
        {
            var usuario = db.Usuarios.Where(u => u.Email.Equals(email)).FirstOrDefault();

            if (usuario.Perfil.Equals(Enumerador.Perfil.Porteiro.ToString()))
            {
                List<Local> list = new List<Local>();

                foreach (var item in usuario.MappedLocal)
                {
                    Local local = new Local();
                    local.DataCadastro = item.DataCadastro;
                    local.LocalId = item.LocalId;
                    local.Nome = item.Nome;
                    local.Sigla = item.Sigla;

                    list.Add(local);
                }

                return list.ToList();
            }

            var allItens = db.Locais.ToList();
            allItens.RemoveAt(0);

            return allItens.ToList();
        }
    }
}
