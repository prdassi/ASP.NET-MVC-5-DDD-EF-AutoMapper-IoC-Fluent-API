using HHT.Domain.Entities;
using System;
using System.Collections.Generic;

namespace HHT.Domain.Interfaces.Services
{
    public interface IEmpresaService : IServiceBase<Empresa>
    {
        Empresa ObterDocumento(int empresaId);

        void SalvarServicosSelecionados(Empresa empresa);

        void SalvarLocaisSelecionados(Empresa empresa);

        Empresa ObterServicosPorId(Empresa empresa);

        void RemoverEspecifico(int empresaId);

        Usuario ObterUsuarioVinculado(int empresaId);

        Contratado ObterContratadoVinculado(int? empresaId, int? usuarioId);

        DateTime? ObterMenorDataValidade(int empresaId);

        IEnumerable<Empresa> ObterPorLocal(int localId);

        IEnumerable<Empresa> ObterPorPerfil(string role, string userName);
    }
}
