using System;
using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Application
{
    public class PontoAppService : AppServiceBase<Ponto>, IPontoAppService
    {
        private readonly IPontoService _pontoService;

        public PontoAppService(IPontoService pontoService) : base(pontoService)
        {
            _pontoService = pontoService;
        }

        public IEnumerable<Ponto> ObterInconsistencias(int localId, int empresaId, int? contratadoId, int ano, int mes, int? dia, int usuarioIdLogado)
        {
            return _pontoService.ObterInconsistencias(localId, empresaId, contratadoId, ano, mes, dia, usuarioIdLogado);
        }

        //public List<AjustarPonto> ObterAjustesPonto(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        //{
        //    return _pontoService.ObterAjustesPonto(localId, empresaId, contratadoId, ano, mes, dia);
        //}

        public IEnumerable<Ponto> ObterRegistroDia(int contratadoId, int ano, int mes, int dia)
        {
            return _pontoService.ObterRegistroDia(contratadoId, ano, mes, dia);
        }

        public IEnumerable<Ponto> ObterPorFiltro(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        {
            return _pontoService.ObterPorFiltro(localId, empresaId, contratadoId, ano, mes, dia);
        }

        public void RemoveById(int contratadoId)
        {
            _pontoService.RemoveById(contratadoId);
        }
    }
}
