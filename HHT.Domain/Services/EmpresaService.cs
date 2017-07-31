using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class EmpresaService : ServiceBase<Empresa>, IEmpresaService
    {
        private readonly IEmpresaRepository _empresaRepository;

        public EmpresaService(IEmpresaRepository empresaRepository) : base (empresaRepository)
        {
            _empresaRepository = empresaRepository;
        }
        
        public Empresa ObterDocumento(int empresaId)
        {
            return _empresaRepository.ObterDocumento(empresaId);
        }

        public void SalvarServicosSelecionados(Empresa empresa)
        {
            _empresaRepository.SalvarServicosSelecionados(empresa);
        }

        public void SalvarLocaisSelecionados(Empresa empresa)
        {
            _empresaRepository.SalvarLocaisSelecionados(empresa);
        }

        public Empresa ObterServicosPorId(Empresa empresa)
        {
            return _empresaRepository.ObterServicosPorId(empresa);
        }

        public void RemoverEspecifico(int empresaId)
        {
            _empresaRepository.RemoverEspecifico(empresaId);
        }
        
        public Usuario ObterUsuarioVinculado(int empresaId)
        {
            return _empresaRepository.ObterUsuarioVinculado(empresaId);
        }

        public Contratado ObterContratadoVinculado(int? empresaId, int? usuarioId)
        {
            return _empresaRepository.ObterContratadoVinculado(empresaId, usuarioId);
        }

        public DateTime? ObterMenorDataValidade(int empresaId)
        {
            return _empresaRepository.ObterMenorDataValidade(empresaId);
        }

        public IEnumerable<Empresa> ObterPorLocal(int localId)
        {
            return _empresaRepository.ObterPorLocal(localId);
        }

        public IEnumerable<Empresa> ObterPorPerfil(string role, string userName)
        {
            return _empresaRepository.ObterPorPerfil(role, userName);
        }
    }
}
