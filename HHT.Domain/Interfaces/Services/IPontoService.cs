using HHT.Domain.Entities;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Services
{
    public interface IPontoService : IServiceBase<Ponto>
    {
        IEnumerable<Ponto> ObterInconsistencias(int localId, int empresaId, int? contratadoId, int ano, int mes, int? dia, int usuarioIdLogado);

        IEnumerable<Ponto> ObterRegistroDia(int contratadoId, int ano, int mes, int dia);

        IEnumerable<Ponto> ObterPorFiltro(int localId, int empresaId, int contratadoId, int ano, int mes, int dia);

        void RemoveById(int contratadoId);
    }
}
