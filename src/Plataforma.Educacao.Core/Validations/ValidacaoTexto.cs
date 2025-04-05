using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Core.Validations
{
    public static class ValidacaoTexto
    {
        public static void DeveSerDiferenteDe<T>(string valor, string comparacao, string mensagem, ResultadoValidacao<T> resultado) where T : class
        {
            if (valor != comparacao)
                resultado.AdicionarErro(mensagem);
        }

        public static void DevePossuirConteudo<T>(string valor, string mensagem, ResultadoValidacao<T> resultado) where T : class
        {
            if (string.IsNullOrWhiteSpace(valor))
                resultado.AdicionarErro(mensagem);
        }

        public static void DevePossuirTamanho<T>(string valor, string mensagem, int tamanhoMinimo, int? tamanhoMaximo, ResultadoValidacao<T> resultado) where T : class
        {
            tamanhoMinimo = tamanhoMinimo == 0 ? 1 : tamanhoMinimo;

            if ((valor?.Length ?? 0) < tamanhoMinimo || (tamanhoMaximo.HasValue && valor.Length > tamanhoMaximo.Value))
                resultado.AdicionarErro(mensagem);
        }
    }
}
