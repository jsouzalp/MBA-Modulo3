namespace Plataforma.Educacao.Core.Messages.Comunications.FaturamentoCommands;
public class RealizarPagamentoCommand : CommandRaiz
{
    public Guid MatriculaCursoId { get; init; }
    public decimal Valor { get; init; }
    public string NumeroCartao { get; init; }
    public string NomeTitularCartao { get; init; }
    public string ValidadeCartao { get; init; }
    public string CvvCartao { get; init; }

    public RealizarPagamentoCommand(Guid matriculaId, decimal valor, string numeroCartao, string nomeTitularCartao, string validadeCartao, string cvvCartao)
    {
        DefinirRaizAgregacao(matriculaId);
        MatriculaCursoId = matriculaId;
        Valor = valor;
        NumeroCartao = numeroCartao;
        NomeTitularCartao = nomeTitularCartao;
        ValidadeCartao = validadeCartao;
        CvvCartao = cvvCartao;
    }
}
