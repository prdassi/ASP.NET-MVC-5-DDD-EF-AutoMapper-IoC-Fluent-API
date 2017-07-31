using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Collections;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class LocalService : ServiceBase<Local>, ILocalService
    {
        private readonly ILocalRepository _localRepository;

        public LocalService(ILocalRepository localRepository) : base (localRepository)
        {
            _localRepository = localRepository;
        }

        public int GetNull()
        {
            return _localRepository.GetNull();
        }

        public int[] ObterLocaisEmpresaSeleciponados(Empresa empresa)
        {
            return _localRepository.ObterLocaisEmpresaSeleciponados(empresa);
        }

        public IEnumerable<Local> ObterPorPerfil(string role, string userName)
        {
            return _localRepository.ObterPorPerfil(role, userName);
        }

        public List<Local> ObterPorEmail(string email)
        {
            return _localRepository.ObterPorEmail(email);
        }
    }
}
