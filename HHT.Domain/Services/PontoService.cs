using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class PontoService : ServiceBase<Ponto>, IPontoService
    {
        private readonly IPontoRepository _pontoRepository;

        public PontoService(IPontoRepository pontoRepository) : base(pontoRepository)
        {
            _pontoRepository = pontoRepository;
        }

        public IEnumerable<Ponto> ObterInconsistencias(int localId, int empresaId, int? contratadoId, int ano, int mes, int? dia, int usuarioIdLogado)
        {
            return _pontoRepository.ObterInconsistencias(localId, empresaId, contratadoId, ano, mes, dia, usuarioIdLogado);
        }

        //public List<AjustarPonto> ObterAjustesPonto(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        //{
        //    return _pontoRepository.ObterAjustesPonto(localId, empresaId, contratadoId, ano, mes, dia);
        //}

        public IEnumerable<Ponto> ObterRegistroDia(int contratadoId, int ano, int mes, int dia)
        {
            return _pontoRepository.ObterRegistroDia(contratadoId, ano, mes, dia);
        }

        public IEnumerable<Ponto> ObterPorFiltro(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        {
            return _pontoRepository.ObterPorFiltro(localId, empresaId, contratadoId, ano, mes, dia);
        }

        public void RemoveById(int contratadoId)
        {
            _pontoRepository.RemoveById(contratadoId);
        }
    }
}
