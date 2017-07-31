using HHT.Domain.Entities;
using HHT.Domain.Interfaces.Repositories;
using HHT.Infra.CrossCutting.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HHT.Infra.Data.Repositories
{
    public class PontoRepository : RepositoryBase<Ponto>, IPontoRepository
    {
        public IEnumerable<Ponto> ObterInconsistencias(int localId, int empresaId, int? contratadoId, int ano, int mes, int? dia, int usuarioIdLogado)
        {
            try
            {
                var result = db.Pontos.Include("Contratado")
                                      .Include("Contratado.Empresa")
                                      .AsQueryable()
                                      .AsEnumerable();

                if (localId != 0)
                    result = result.Where(p => p.LocalId == localId);
                if (empresaId != 0)
                    result = result.Where(p => p.Contratado.Empresa.EmpresaId == empresaId);
                if (contratadoId != null)
                {
                    if (contratadoId != 0)
                    {
                        result = result.Where(p => p.ContratadoId == contratadoId);
                    }

                }

                if (ano != 0)
                    result = result.Where(p => p.DataRegistro.Year == ano);
                if (mes != 0)
                    result = result.Where(p => p.DataRegistro.Month == mes);
                if (dia != null)
                {
                    if (dia != 0)
                    {
                        result = result.Where(p => p.DataRegistro.Day == dia);
                    }
                }


                //Ids dos Contratados
                var listaContratadoIds = result.GroupBy(i => i.ContratadoId).Select(s => s.First().ContratadoId).ToList();

                //Dias do ponto de cada Contratado
                var resultOrder = result.OrderBy(r => r.DataRegistro);

                List<Ponto> listaInconsistencia = new List<Ponto>();

                foreach (var itemContratadoId in listaContratadoIds)
                {
                    List<IGrouping<int, Ponto>> listaDias = result.Where(i => i.ContratadoId == itemContratadoId).GroupBy(i => i.DataRegistro.Day).ToList();

                    List<IGrouping<int, Ponto>> diaSeguinte = listaDias.ToList();
                    foreach (var dias in listaDias)
                    {
                        diaSeguinte.Remove(dias);

                        if (dias.Count() == 3 &&
                            dias.ElementAt(0).Registro.Equals("Entrada") &&
                             dias.ElementAt(1).Registro.Equals("Saída") &&
                              dias.ElementAt(2).Registro.Equals("Entrada") && ProximoDia(diaSeguinte))
                        {

                            Ponto pontoSaida = new Ponto()
                            {
                                DataRegistro = dias.ElementAt(0).DataRegistro,
                                HoraRegistro = dias.ElementAt(0).DataRegistro.Date.AddHours(23).AddMinutes(59).AddSeconds(59),
                                Registro = "Saída",
                                ContratadoId = dias.ElementAt(0).ContratadoId,
                                LocalId = dias.ElementAt(0).LocalId,
                                ModoRegistro = "Sistema",
                                UsuarioId = usuarioIdLogado
                                
                            };

                            Ponto pontoEntrada = new Ponto()
                            {
                                DataRegistro = diaSeguinte.FirstOrDefault().ElementAt(0).DataRegistro,
                                HoraRegistro = diaSeguinte.FirstOrDefault().ElementAt(0).DataRegistro.Date.AddHours(00).AddMinutes(00).AddSeconds(00),
                                Registro = "Entrada",
                                ContratadoId = diaSeguinte.FirstOrDefault().ElementAt(0).ContratadoId,
                                LocalId = diaSeguinte.FirstOrDefault().ElementAt(0).LocalId,
                                ModoRegistro = "Sistema",
                                UsuarioId = usuarioIdLogado
                            };

                            db.Pontos.Add(pontoEntrada);

                            db.Pontos.Add(pontoSaida);

                            db.SaveChanges();

                        }
                        else if (dias.Count() > 3)
                        {
                            if (!VerificaSaida(dias, diaSeguinte))
                            {
                                listaInconsistencia.Add(this.PontoComErro(itemContratadoId, dias));
                            }
                        }
                        else
                        {
                            listaInconsistencia.Add(this.PontoComErro(itemContratadoId, dias));
                        }
                    }
                }

                return listaInconsistencia;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Ponto> ObterRegistroDia(int contratadoId, int ano, int mes, int dia)
        {
            try
            {
                var result = db.Pontos.Include("Contratado")
                                      .Include("Contratado.Empresa")
                                      .AsQueryable()
                                      .AsEnumerable();

                if (contratadoId != 0)
                {
                    result = result.Where(p => p.ContratadoId == contratadoId);
                }
                if (ano > 0)
                {
                    result = result.Where(p => p.DataRegistro.Year == ano);
                }
                if (mes > 0)
                {
                    result = result.Where(p => p.DataRegistro.Month == mes);
                }
                if (dia > 0)
                {
                    result = result.Where(p => p.DataRegistro.Day == dia);
                }

                return result.OrderBy(p => p.HoraRegistro).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<Ponto> ObterPorFiltro(int localId, int empresaId, int contratadoId, int ano, int mes, int dia)
        {
            try
            {
                var result = db.Pontos.Include("Contratado")
                                      .Include("Contratado.Empresa")
                                      .AsQueryable()
                                      .AsEnumerable();

                if (localId != 0)
                    result = result.Where(p => p.LocalId == localId);
                if (empresaId != 0)
                    result = result.Where(p => p.Contratado.Empresa.EmpresaId == empresaId);

                if (contratadoId != 0)
                {
                    result = result.Where(p => p.ContratadoId == contratadoId);
                }

                if (ano != 0)
                    result = result.Where(p => p.DataRegistro.Year == ano);
                if (mes != 0)
                    result = result.Where(p => p.DataRegistro.Month == mes);

                if (dia != 0)
                {
                    result = result.Where(p => p.DataRegistro.Day == dia);
                }


                //Dias do Contratado
                var listaContratadoId = result.GroupBy(i => i.ContratadoId).ToList();


                List<Ponto> listaPontoContratado = new List<Ponto>();

                foreach (var itemContratado in listaContratadoId)
                {
                    // IGrouping<int, Ponto> listaDias = result.Where(i => i.DataRegistro.Day == itemContratado).GroupBy(i => i.DataRegistro.Day).FirstOrDefault();

                    listaPontoContratado.Add(itemContratado.FirstOrDefault());
                }

                return listaPontoContratado;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private TimeSpan HorasTrabalhadas(IGrouping<int, Ponto> ponto)
        {
            //total HHT
            var totalHHT = ponto.OrderBy(p => p.HoraRegistro).Last().HoraRegistro.Value
                            .Subtract(ponto.OrderBy(p => p.HoraRegistro).First().HoraRegistro.Value);

            return totalHHT;
        }

        private bool VerificaSaida(IGrouping<int, Ponto> dias, List<IGrouping<int, Ponto>> diaSeguinte)
        {

            diaSeguinte.Remove(dias);

            List<Dictionary<string, string>> listaPonto = new List<Dictionary<string, string>>();

            var t = dias.OrderBy(i => i.HoraRegistro).ToList();

            if (t.Count() > 2 && t.ElementAt(0).Registro.Equals("Entrada") &&
                t.ElementAt(1).Registro.Equals("Saída") &&
                ProximoDia(diaSeguinte))
            {
                t.RemoveAt(0);
                t.RemoveAt(1);
            }

            if (t.Count() > 0)
            {
                if (t.ElementAt(0).Registro.Equals("Saída"))
                    return false;
                else if (t.ElementAt(t.Count() - 1).Registro.Equals("Saída"))
                    return true;
                else if (t.ElementAt(t.Count() - 1).Registro.Equals("Entrada") && ProximoDia(diaSeguinte))
                    return true;
                else
                    return false;
            }

            return false;
        }

        private bool ProximoDia(List<IGrouping<int, Ponto>> diaSeguinte)
        {
            if (diaSeguinte.Count() > 0)
            {
                var t = diaSeguinte.FirstOrDefault().OrderBy(i => i.HoraRegistro).ToList();

                if (t.ElementAt(0).Registro.Equals("Saída"))
                    return true;
            }

            return false;
        }

        private Ponto PontoComErro(int contratadoId, IGrouping<int, Ponto> dias)
        {
            //Ponto com problema
            Ponto ponto = new Ponto();
            ponto.ContratadoId = contratadoId;

            Contratado contratado = new Contratado();
            contratado.Nome = dias.First().Contratado.Nome;
            contratado.RG = dias.First().Contratado.RG;
            contratado.Ativo = dias.First().Contratado.Ativo;

            ponto.Contratado = contratado;

            Empresa empresa = new Empresa();
            empresa.Nome = dias.First().Contratado.Empresa.Nome;

            ponto.Contratado.Empresa = empresa;

            ponto.DataRegistro = dias.First().DataRegistro;
            ponto.HoraRegistro = null; //Não sabemos a hora que está inconsistente

            Local local = new Local();
            local.LocalId = dias.First().LocalId;
            local.Sigla = dias.First().Local.Sigla;

            ponto.Local = local;

            ponto.Registro = ""; //Não sabemos qual registro que está inconsistente
            ponto.PontoId = dias.First().PontoId;

            Funcao funcao = new Funcao();
            funcao.Nome = dias.First().Contratado.Funcao.Nome;

            ponto.Contratado.Funcao = funcao;

            return ponto;
        }

        public void RemoveById(int contratadoId)
        {
            try
            {
                db.Pontos.RemoveRange(db.Pontos.Where(x => x.ContratadoId == contratadoId));
                db.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}