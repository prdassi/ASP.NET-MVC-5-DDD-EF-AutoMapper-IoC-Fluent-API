using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Domain.Interfaces.Services;
using System.Collections.Generic;

namespace HHT.Domain.Services
{
    public class AjustePontoService : ServiceBase<AjustePonto>, IAjustePontoService
    {
        private readonly IAjustePontoRepository _ajustarPontoRepository;

        public AjustePontoService(IAjustePontoRepository ajustarPontoRepository) : base(ajustarPontoRepository)
        {
            _ajustarPontoRepository = ajustarPontoRepository;
        }

        public List<AjustePonto> ObterPontoDetalhado(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        {
            return _ajustarPontoRepository.ObterPontoDetalhado(localId, empresaId, contratadoId, ano, mes, dia);
        }
    }
}
