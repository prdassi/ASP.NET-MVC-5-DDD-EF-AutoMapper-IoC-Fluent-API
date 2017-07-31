using System;
using System.Linq;

namespace HHT.Infra.CrossCutting.Helper
{
    public class GeradorString
    {
        public static string RandomAlfanumerico(string tamanho)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, tamanho.Count()/2)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }
    }
}
