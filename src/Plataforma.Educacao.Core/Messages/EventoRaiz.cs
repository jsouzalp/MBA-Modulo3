using MediatR;

namespace Plataforma.Educacao.Core.Messages;
public abstract class EventoRaiz : INotification
{
    public Guid RaizAgregacao { get; internal set; }
    public DateTime DataHora { get; internal set; }

    public EventoRaiz()
    {
        DataHora = DateTime.Now;
    }
}
