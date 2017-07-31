using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Infra.CrossCutting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HHT.Infra.Data.Repositories
{
    public class AjustePontoRepository : RepositoryBase<AjustePonto>, IAjustePontoRepository
    {
        public List<AjustePonto> ObterPontoDetalhado(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        {
            var result = db.Pontos.Include("Contratado")
                                      .Include("Contratado.Empresa")
                                      .AsQueryable()
                                      .AsEnumerable();

            if (localId != 0)
            {
                result = result.Where(p => p.LocalId == localId);
            }
            if (empresaId != 0)
            {
                result = result.Where(p => p.Contratado.Empresa.EmpresaId == empresaId);
            }
            if (contratadoId > 0)
            {
                result = result.Where(p => p.ContratadoId == contratadoId);
            }
            if (ano != 0)
            {
                result = result.Where(p => p.DataRegistro.Year == ano);
            }
            if (mes != 0)
            {
                result = result.Where(p => p.DataRegistro.Month == mes);
            }
            if (dia > 0)
            {
                result = result.Where(p => p.DataRegistro.Day == dia);
            }

            //Ids dos Contratados
            var listaContratadoIds = result.GroupBy(i => i.ContratadoId).Select(s => s.First().ContratadoId).ToList();


            //Dias do Contratado
            var listaDiaContratado = result.GroupBy(i => i.DataRegistro.Day).ToList();
            
            List<AjustePonto> listaAjustePonto = new List<AjustePonto>();

            foreach (var itemContratado in listaDiaContratado)
            {
                AjustePonto ajustePonto = new AjustePonto();
                ajustePonto.ContratadoId = itemContratado.First().ContratadoId;
                ajustePonto.Nome = itemContratado.First().Contratado.Nome;
                ajustePonto.RG = itemContratado.First().Contratado.RG;
                ajustePonto.Ano = itemContratado.First().DataRegistro.Year;
                ajustePonto.Mes = FormatarAnoMesDia.MesPorExtenso(itemContratado.First().DataRegistro.Month);
                ajustePonto.NumeroDia = itemContratado.First().DataRegistro.Day;

                if (itemContratado.Any(i => i.ModoRegistro.Equals(Enumerador.ModoRegisto.Manual.ToString())))
                {
                    ajustePonto.HHT = Enumerador.ModoRegisto.Manual.ToString();
                }
                else
                {
                    ajustePonto.HHT = Enumerador.ModoRegisto.Automático.ToString();
                }

                ajustePonto.TotalHHT = HorasTrabalhadas(itemContratado);

                listaAjustePonto.Add(ajustePonto);
            }

            if (dia == 0)
            {
                //Preencher os dias nulo, quando não houver ponto
                for (var diasDoMes = 1; diasDoMes < FormatarAnoMesDia.Dia(mes, ano).Count(); diasDoMes++)
                {
                    var item = listaAjustePonto.Any(d => d.NumeroDia.Equals(diasDoMes));

                    if (!item)
                    {
                        AjustePonto ajustePonto = new AjustePonto();
                        ajustePonto.ContratadoId = listaAjustePonto.First().ContratadoId;
                        ajustePonto.Nome = listaAjustePonto.First().Nome;
                        ajustePonto.RG = listaAjustePonto.First().RG;
                        ajustePonto.Ano = ano;
                        ajustePonto.Mes = FormatarAnoMesDia.MesPorExtenso(mes);
                        ajustePonto.NumeroDia = diasDoMes;
                        ajustePonto.HHT = "";
                        ajustePonto.TotalHHT = null;

                        listaAjustePonto.Add(ajustePonto);
                    }
                }
            }            

            return listaAjustePonto.OrderBy(l => l.NumeroDia).ToList();
        }

        private TimeSpan HorasTrabalhadas(IGrouping<int, Ponto> ponto)
        {
            var primeiroRegistro = ponto.OrderByDescending(p => p.HoraRegistro).Last().HoraRegistro.Value;
            var ultimoRegistro = ponto.OrderByDescending(p => p.HoraRegistro).First().HoraRegistro.Value;

            return ultimoRegistro.Subtract(primeiroRegistro);
        }
    }
}