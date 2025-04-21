namespace Plataforma.Educacao.Core.Exceptions
{
    public class DomainException : Exception
    {
        //public DomainException() { }
        //public DomainException(string mensagem) : base(mensagem) { }
        //public DomainException(string mensagem, Exception innerException) : base(mensagem, innerException) { }

        public IReadOnlyCollection<string> Errors { get; }

        public DomainException(string mensagem) : base(mensagem)
        {
            Errors = [mensagem];
        }

        public DomainException(IEnumerable<string> mensagens) : base(string.Join("; ", mensagens))
        {
            Errors = mensagens.ToList().AsReadOnly();
        }

        public DomainException(string mensagem, Exception innerException) : base(mensagem, innerException)
        {
            Errors = [mensagem];
        }
    }
}
