namespace HHT.Infra.CrossCutting.Helper
{
    public class Enumerador
    {
        public enum Registo
        {
            Entrada,
            Saida
        }

        public enum ModoRegisto
        {
            Automático,
            Manual
        }

        public enum IdentificacaoDocumento
        {
            Vencido,
            AVencer
        }

        public enum Associado
        {
            Empresa,
            Contratado
        }

        public enum TipoDocumento
        {
            Documento,
            Integração,
            Treinamento
        }

        public enum Perfil
        {
            Porteiro,
            Empresa,
            Seguranca,
            Funcional,
            TI,
            Gerente
        }
    }
}
