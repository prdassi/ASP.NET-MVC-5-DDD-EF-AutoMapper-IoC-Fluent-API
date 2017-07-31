using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace HHT.Infra.Data.Repositories
{
    public class ServicoRepository : RepositoryBase<Servico>, IServicoRepository
    {
        public int[] ObterServicosSelecionados(Empresa empresa)
        {
            int[] myIndices = new int[empresa.MappedServico.Count];

            List<int> list = new List<int>();

            foreach (var selecao in empresa.MappedServico)
            {
                list.Add(selecao.Servico.ServicoId);
            }

            return list.Cast<int>().ToArray();
        }
    }
}
