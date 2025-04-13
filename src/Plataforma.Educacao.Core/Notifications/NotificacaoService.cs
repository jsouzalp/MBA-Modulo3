namespace Plataforma.Educacao.Core.Notifications
{
    public class NotificacaoService : INotificacaoService
    {
        private ICollection<Notificacao> _notificacao;

        public NotificacaoService()
        {
            _notificacao = [];
        }

        public bool PossuiNotificacoes() => _notificacao != null && _notificacao.Any();
        public ICollection<Notificacao> ObterNotificacoes() => _notificacao;

        public void Notificar(Notificacao notificacao)
        {
            _notificacao.Add(notificacao);
        }

        public void Notificar(string Notificacao)
        {
            Notificar(new Notificacao(Notificacao));
        }
    }
}
