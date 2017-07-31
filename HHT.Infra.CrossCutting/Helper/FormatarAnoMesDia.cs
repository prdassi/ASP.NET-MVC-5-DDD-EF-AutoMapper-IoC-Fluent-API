using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace HHT.Infra.CrossCutting.Helper
{
    public class FormatarAnoMesDia
    {
        public static IDictionary<int, int> Ano()
        {
            IDictionary<int, int> ano = new Dictionary<int, int>();

            ano.Add(DateTime.Now.Year, DateTime.Now.Year);
            ano.Add(DateTime.Now.Year - 1, DateTime.Now.Year - 1);
            ano.Add(DateTime.Now.Year - 2, DateTime.Now.Year - 2);

            return ano;
        }

        public static IEnumerable<object> Mes(int anoAtual)
        {
            IDictionary<int, string> mes = new Dictionary<int, string>();

            if (anoAtual < DateTime.Now.Year)
            {
                for (var x = 1; x <= 12; x++)
                {
                    string mesMinusculo = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x);
                    mes.Add(x, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mesMinusculo));
                }
            }
            else
            {
                for (var x = 1; x <= DateTime.Now.Month; x++)
                {
                    string mesMinusculo = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(x);
                    mes.Add(x, CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mesMinusculo));
                }
            }

            var meses = mes.Select(x => new
            {
                mesId = x.Key,
                nome = x.Value
            });

            return meses;
        }

        public static IEnumerable<object> Dia(int mes, int ano)
        {
            try
            {
                IDictionary<int, int> dias = new Dictionary<int, int>();

                var ultimoDia = DateTime.DaysInMonth(ano, mes);

                if (ano < DateTime.Now.Year)
                {
                    for (var x = 1; x <= Convert.ToInt16(ultimoDia); x++)
                    {
                        dias.Add(x, x);
                    }
                }
                else
                {
                    if (mes < DateTime.Now.Month)
                    {
                        for (var x = 1; x <= Convert.ToInt16(ultimoDia); x++)
                        {
                            dias.Add(x, x);
                        }
                    }
                    else
                    {
                        for (var x = 1; x < Convert.ToInt16(DateTime.Now.Day); x++)
                        {
                            dias.Add(x, x);
                        }
                    }
                }

                var diasMes = dias.Select(x => new
                {
                    diaId = x.Key,
                    numDia = x.Value
                });

                return diasMes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string MesPorExtenso(int mes)
        {
            string mesMinusculo = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes);
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mesMinusculo);
        }

        public static int NumeroMes(string nomeMes)
        {
            return DateTime.Parse(nomeMes + " 01, 1900").Month;
        }
    }
}
