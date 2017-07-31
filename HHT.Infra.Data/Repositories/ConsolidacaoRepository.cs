using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Infra.CrossCutting.Helper;
using HHT.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using static HHT.Domain.Entities.Consolidacao;

namespace HHT.Infra.Data.Repositories
{
    public class ConsolidacaoRepository : RepositoryBase<Consolidacao>, IConsolidacaoRepository
    {
        public IEnumerable<Consolidacao> ObterResultados(int localId, int empresaId, int ano, int mes)
        {

            var result = db.Pontos
                                .Include("Local")
                                .Include("Contratado")
                                .Include("Contratado.Empresa")
                                .Where(p => p.LocalId == localId && p.DataRegistro.Year == ano)
                                .AsQueryable()
                                .AsEnumerable();


           
            List<Consolidacao> listaConsolidacao = new List<Consolidacao>();

            // Busca empresa específca
            if (empresaId != 0)
            {
                result = result.Where(p => p.Contratado.Empresa.EmpresaId == empresaId);
            }
            if (mes != 0)
            {
                result = result.Where(p => p.DataRegistro.Month == mes);
            }

            var listaEmpresas = result.GroupBy(i => i.Contratado.Empresa.Nome).ToList();



            if (listaEmpresas.Count > 0)
            {

                foreach (var empresa in listaEmpresas)
                {
                    Consolidacao consolidacao = new Consolidacao();
                    consolidacao.Empresa = empresa.First().Contratado.Empresa.Nome;
                    consolidacao.Gestor = empresa.First().Contratado.Empresa.NomeGestorAES;

                    consolidacao.Meses = new List<Consolidacao.Mes>();
                    List<Mes> listMesUnico = new List<Mes>();

                    var listaMeses = empresa.OrderBy(o => o.DataRegistro.Month).GroupBy(i => i.DataRegistro.Month).ToList();

                    if (mes > 0)
                    {
                        var listItensMes = listaMeses.Select(s => s.GroupBy(g => g.Contratado.ContratadoId)).FirstOrDefault();

                        Mes mesConsolidado = new Mes();

                        foreach (var item in listItensMes)
                        {
                            mesConsolidado.Nome = FormatarAnoMesDia.MesPorExtenso(empresa.First().DataRegistro.Month);
                            mesConsolidado.NumeroFuncionario = empresa.GroupBy(c => c.ContratadoId).Count();
                            mesConsolidado.HHT = mesConsolidado.HHT + CalculaHoraTrabalhada(item);
                        }

                        listMesUnico.Add(mesConsolidado);

                        consolidacao.Meses = listMesUnico;
                    }
                    else
                    {
                        consolidacao.Meses = AnalisaMesCorrente(listaMeses, ano, mes);

                    }

                    listaConsolidacao.Add(consolidacao);
                }
            }

            var NovaLista = listaConsolidacao.OrderBy(o => o.Meses.First().Nome);

            return listaConsolidacao;
        }

        private int CalculaHoraTrabalhada(IGrouping<int, Ponto> item)
        {
            var diasMes = item.GroupBy(p => p.DataRegistro.Date.Day).ToList();

            int totalHoraTrabalhada = 0;

            foreach (var dia in diasMes)
            {
                var ultimoRegistro = dia.OrderByDescending(d => d.HoraRegistro.Value.Hour).First();
                var primeiroRegistro = dia.OrderBy(d => d.HoraRegistro.Value.Hour).First();

                totalHoraTrabalhada = totalHoraTrabalhada + (ultimoRegistro.HoraRegistro.Value.Subtract(primeiroRegistro.HoraRegistro.Value).Hours);
            }

            return totalHoraTrabalhada;
        }

        private List<Mes> AnalisaMesCorrente(IList<IGrouping<int, Ponto>> listaMes, int ano, int mes)
        {
            List<Mes> listaMeses = new List<Mes>();
            List<Mes> ListaMesesConsolidados = new List<Mes>();
            var listItensMes = listaMes.Select(s => s.GroupBy(g => g.ContratadoId)).FirstOrDefault();

            foreach (var item in listItensMes)
            {
                var meses = item.GroupBy(p => p.DataRegistro.Date.Month).ToList();

                if (ano < DateTime.Now.Year)
                {
                    for (var x = 0; x < 12; x++)
                    {
                        Mes mesConsolidado = new Mes();

                        listaMeses.Add(mesConsolidado);
                    }
                }
                else
                {
                    for (var x = 0; x < DateTime.Now.Month; x++)
                    {
                        var mesExistente = meses.Where(m => m.Select(s => s.DataRegistro.Month.Equals(x + 1)).FirstOrDefault());

                        Mes mesConsolidado = new Mes();
                        mesConsolidado.Nome = FormatarAnoMesDia.MesPorExtenso(x + 1);
                        mesConsolidado.NumeroFuncionario = 0;
                        mesConsolidado.HHT = 0;

                        if (mesExistente.Count() > 0)
                        {
                            mesConsolidado.Nome = FormatarAnoMesDia.MesPorExtenso(item.First().DataRegistro.Month);
                            mesConsolidado.NumeroFuncionario = mesConsolidado.NumeroFuncionario + item.GroupBy(c => c.ContratadoId).Count();
                            mesConsolidado.HHT = mesConsolidado.HHT + CalculaHoraTrabalhada(item);

                        }

                        listaMeses.Add(mesConsolidado);
                    }


                }
            }


            for (int k = 0; k < DateTime.Now.Month; k++)
            {
                var mesRelatorio = new Mes();
                var nomeMes = FormatarAnoMesDia.MesPorExtenso(k + 1);
                var listaMesesRelatorio = listaMeses.Where(x => x.Nome == nomeMes).ToList();

                foreach (var itemMesReport in listaMesesRelatorio)
                {
                    mesRelatorio.Nome = itemMesReport.Nome;
                    mesRelatorio.HHT += itemMesReport.HHT;
                    mesRelatorio.NumeroFuncionario += itemMesReport.NumeroFuncionario;
                }

                ListaMesesConsolidados.Add(mesRelatorio);

            }


            return ListaMesesConsolidados;
        }
    }
}
