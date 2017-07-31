using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace HHT.Application
{
    public class EmpresaAppService : AppServiceBase<Empresa>, IEmpresaAppService
    {
        private readonly IEmpresaService _empresaService;

        public EmpresaAppService(IEmpresaService empresaService) : base (empresaService)
        {
            _empresaService = empresaService;
        }

        public Empresa ObterDocumento(int empresaId)
        {
            return _empresaService.ObterDocumento(empresaId);
        }

        public void SalvarServicosSelecionados(Empresa empresa)
        {
            _empresaService.SalvarServicosSelecionados(empresa);
        }

        public void SalvarLocaisSelecionados(Empresa empresa)
        {
            _empresaService.SalvarLocaisSelecionados(empresa);
        }

        public Empresa ObterServicosPorId(Empresa empresa)
        {
            return _empresaService.ObterServicosPorId(empresa);
        }

        public void RemoverEspecifico(int empresaId)
        {
            _empresaService.RemoverEspecifico(empresaId);
        }

        public Usuario ObterUsuarioVinculado(int empresaId)
        {
            return _empresaService.ObterUsuarioVinculado(empresaId);
        }

        public Contratado ObterContratadoVinculado(int? empresaId, int? usuarioId)
        {
            return _empresaService.ObterContratadoVinculado(empresaId, usuarioId);
        }

        public DateTime? ObterMenorDataValidade(int empresaId)
        {
            return _empresaService.ObterMenorDataValidade(empresaId);
        }

        public IEnumerable<Empresa> ObterPorLocal(int localId)
        {
            return _empresaService.ObterPorLocal(localId);
        }

        public IEnumerable<Empresa> ObterPorPerfil(string role, string userName)
        {
            return _empresaService.ObterPorPerfil(role, userName);
        }
    }
}
