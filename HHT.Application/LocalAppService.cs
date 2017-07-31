using HHT.Application.Interface;
using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Application
{
    public class LocalAppService : AppServiceBase<Local>, ILocalAppService
    {
        private readonly ILocalService _localService;

        public LocalAppService(ILocalService localService) : base (localService)
        {
            _localService = localService;
        }

        public int GetNull()
        {
            return _localService.GetNull();
        }

        public int[] ObterLocaisEmpresaSeleciponados(Empresa empresa)
        {
            return _localService.ObterLocaisEmpresaSeleciponados(empresa);
        }

        public IEnumerable<Local> ObterPorPerfil(string role, string userName)
        {
            return _localService.ObterPorPerfil(role, userName);
        }

        public List<Local> ObterPorEmail(string email)
        {
            return _localService.ObterPorEmail(email);
        }
    }
}
