using System;
using System.Linq;

namespace HHT.Infra.CrossCutting.Helper
{
    public class FormatarDataHora
    {
        public static string Data(DateTime dataTime)
        {
            return String.Format("{0:d/M/yyyy}", dataTime);            
        }

        public static string Hora(DateTime dataTime)
        {
            return String.Format("{0:T}", dataTime);
        }
    }
}
