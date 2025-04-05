using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Core.Exceptions
{
    public class DomainException : Exception
    {
        public DomainException() { }
        public DomainException(string mensagem) : base(mensagem) { }
        public DomainException(string mensagem, Exception innerException) : base(mensagem, innerException) { }
    }
}
