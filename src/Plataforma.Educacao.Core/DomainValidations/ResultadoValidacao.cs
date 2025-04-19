using Plataforma.Educacao.Core.Exceptions;

namespace Plataforma.Educacao.Core.DomainValidations
{
    public class ResultadoValidacao<T> where T : class
    {
        private readonly IList<string> _erros;
        private bool _ehValido => _erros.Count == 0;

        public ResultadoValidacao()
        {
            _erros = [];
        }

        public void AdicionarErro(string mensagem)
        {
            if (!string.IsNullOrWhiteSpace(mensagem))
                _erros.Add($"({typeof(T).Name}) {mensagem}");
        }

        public void DispararExcecaoDominioSeInvalido()
        {
            if (!_ehValido)
                throw new DomainException(_erros);
                //throw new DomainException(string.Join(Environment.NewLine, _erros));
        }
    }
}
