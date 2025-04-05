using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Core.Validations
{
    public static class ValidacaoGuid
    {
        public static void DeveSerValido<T>(Guid valor, string mensagem, ResultadoValidacao<T> resultado) where T : class
        {
            if (valor == Guid.Empty)
                resultado.AdicionarErro(mensagem);
        }
    }
}
