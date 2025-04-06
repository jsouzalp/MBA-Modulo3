namespace Plataforma.Educacao.Core.Validations
{
    public static class ValidacaoData
    {
        public static void DeveSerValido<T>(DateTime data, string mensagem, ResultadoValidacao<T> resultado) where T : class
        {
            if (data == DateTime.MinValue || data == DateTime.MaxValue)
                resultado.AdicionarErro(mensagem);
        }

        public static void DeveSerMenorQue<T>(DateTime dataValidacao, DateTime dataLimite, string mensagem, ResultadoValidacao<T> resultado) where T : class
        {
            if (dataValidacao == DateTime.MinValue || dataValidacao == DateTime.MaxValue)
                resultado.AdicionarErro(mensagem);

            if (dataLimite == DateTime.MinValue || dataLimite == DateTime.MaxValue)
                resultado.AdicionarErro(mensagem);

            if (dataValidacao.Date > dataLimite.Date)
                resultado.AdicionarErro(mensagem);
        }

        public static void DeveTerRangeValido<T>(DateTime dataInicial, DateTime dataFinal, string mensagem, ResultadoValidacao<T> resultado) where T : class
        {
            if (dataInicial == DateTime.MinValue || dataInicial == DateTime.MaxValue)
                resultado.AdicionarErro(mensagem);

            if (dataFinal == DateTime.MinValue || dataFinal == DateTime.MaxValue)
                resultado.AdicionarErro(mensagem);

            if (dataInicial.Date > dataFinal.Date)
                resultado.AdicionarErro(mensagem);
        }
    }
}
