namespace Plataforma.Educacao.Core.Notifications
{
    public interface INotificacaoService
    {
        bool PossuiNotificacoes();

        ICollection<Notificacao> ObterNotificacoes();

        void Notificar(Notificacao notification);

        void Notificar(string notification);
    }
}
