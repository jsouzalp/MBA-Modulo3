using Plataforma.Educacao.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plataforma.Educacao.Faturamento.Application.Commands.RealizarPagamento;
public class RealizarPagamentoCommand : CommandRaiz
{
    public Guid MatriculaId { get; init; }
    public decimal Valor { get; init; }
    public string NumeroCartao { get; init; }
    public string NomeTitularCartao { get; init; }
    public string ValidadeCartao { get; init; }
    public string CvvCartao { get; init; }

    public RealizarPagamentoCommand(Guid matriculaId, decimal valor, string numeroCartao, string nomeTitularCartao, string validadeCartao, string cvvCartao)
    {
        DefinirRaizAgregacao(matriculaId);
        MatriculaId = matriculaId;
        Valor = valor;
        NumeroCartao = numeroCartao;
        NomeTitularCartao = nomeTitularCartao;
        ValidadeCartao = validadeCartao;
        CvvCartao = cvvCartao;
    }
}
