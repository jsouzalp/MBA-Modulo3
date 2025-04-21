using Plataforma.Educacao.Core.DomainValidations;

namespace Plataforma.Educacao.Conteudo.Domain.ValueObjects;
public class ConteudoProgramatico
{
    #region Atributos
    public string Finalidade { get; }
    public string Ementa { get; }
    #endregion

    #region Construtores
    // EF Constructor
    protected ConteudoProgramatico() { }

    public ConteudoProgramatico(string finalidade, string ementa)
    {
        Finalidade = finalidade;
        Ementa = ementa;

        ValidarIntegridadeConteudoProgramatico();
    }
    #endregion

    #region Validações
    private void ValidarIntegridadeConteudoProgramatico(string novaFinalidade = null, string novaEmenta = null)
    {
        var finalidade = novaFinalidade ?? Finalidade;
        var ementa = novaEmenta ?? Ementa;

        var validacao = new ResultadoValidacao<ConteudoProgramatico>();
        ValidacaoTexto.DevePossuirConteudo(finalidade, "Finalidade não pode ser vazia ou nula", validacao);
        ValidacaoTexto.DevePossuirTamanho(finalidade, 10, 100, "Finalidade do conteúdo programático deve ter entre 10 e 100 caracteres", validacao);
        ValidacaoTexto.DevePossuirConteudo(ementa, "Ementa do conteúdo programático não pode ser vazia ou nula", validacao);
        ValidacaoTexto.DevePossuirTamanho(ementa, 50, 4000, "Ementa do conteúdo programático deve ter entre 50 e 4000 caracteres", validacao);

        validacao.DispararExcecaoDominioSeInvalido();
    }
    #endregion

    #region Overrides
    public override string ToString() => $"Finalidade: {Finalidade}";
    public override int GetHashCode() => HashCode.Combine(Finalidade, Ementa);

    public override bool Equals(object obj)
    {
        if (obj is not ConteudoProgramatico other) return false;
        return Finalidade == other.Finalidade && Ementa == other.Ementa;
    }
    #endregion
}
