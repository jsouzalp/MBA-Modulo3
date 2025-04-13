using Plataforma.Educacao.Conteudo.Domain.ValueObjects;
using Plataforma.Educacao.Core.Aggregates;
using Plataforma.Educacao.Core.Entities;
using Plataforma.Educacao.Core.Validations;

namespace Plataforma.Educacao.Conteudo.Domain.Entities;
public class Curso : Entidade, IRaizAgregacao
{
    #region Atributos
    public string Nome { get; private set; }
    public decimal Valor { get; private set; }
    public bool Ativo { get; private set; }
    public DateTime? ValidoAte { get; private set; }

    #region Helper only for EF Mapping
    public ConteudoProgramatico ConteudoProgramatico { get; private set; }

    private readonly List<Aula> _aulas = [];
    public IReadOnlyCollection<Aula> Aulas => _aulas;
    #endregion
    #endregion

    #region Construtores
    // EF Constructor
    protected Curso() { }
    public Curso(string nome, 
        decimal valor, 
        DateTime? validoAte, 
        ConteudoProgramatico conteudoProgramatico)
        //ICollection<Aula> aulas = null)
    {
        Nome = nome;
        Valor = valor;
        ValidoAte = validoAte;
        ConteudoProgramatico = conteudoProgramatico;
        //if (aulas != null) { _aulas = new List<Aula>(aulas); }

        ValidarIntegridadeCurso();
    }
    #endregion

    #region Getters
    public int CargaHoraria() => _aulas.Sum(a => a.CargaHoraria);
    public int QuantidadeAulas() => _aulas.Count;
    public bool CursoDisponivel() => Ativo && (!ValidoAte.HasValue || ValidoAte.Value >= DateTime.Now.Date);
    #endregion

    #region Setters
    public void AtivarCurso() => Ativo = true;
    public void DesativarCurso() => Ativo = false;

    public void AlterarNome(string nome)
    {
        ValidarIntegridadeCurso(novoNome: nome);
        Nome = nome;
    }

    public void AlterarValor(decimal valor)
    {
        ValidarIntegridadeCurso(novoValor: valor);
        Valor = valor;
    }

    public void AlterarValidadeCurso(DateTime? validoAte)
    {
        ValidarIntegridadeCurso(novoValidoAte: validoAte);
        ValidoAte = validoAte;
    }

    public void AlterarConteudoProgramatico(ConteudoProgramatico conteudoProgramatico)
    {
        ValidarIntegridadeCurso(novoConteudoProgramatico: conteudoProgramatico);
        ConteudoProgramatico = conteudoProgramatico;
    }

    public void AdicionarAula(string descricao, short cargaHoraria, byte ordemAula)
    {
        ValidarOrdemAula(ordemAula);
        _aulas.Add(new Aula(Id, descricao, cargaHoraria, ordemAula));
    }

    public void RemoverAula(Aula aula)
    {
        if (_aulas == null) { return; }
        _aulas.Remove(aula);
    }
    #endregion

    #region Validações
    private void ValidarIntegridadeCurso(string novoNome = null, decimal? novoValor = null, DateTime? novoValidoAte = null, ConteudoProgramatico novoConteudoProgramatico = null)
    {
        var nome = novoNome ?? Nome;
        var valor = novoValor ?? Valor;
        var validoAte = novoValidoAte ?? ValidoAte;
        var conteudoProgramatico = novoConteudoProgramatico ?? ConteudoProgramatico;

        var validacao = new ResultadoValidacao<Curso>();
        ValidacaoTexto.DevePossuirConteudo(nome, "Nome do curso não pode ser vazio ou nulo", validacao);
        ValidacaoTexto.DevePossuirTamanho(nome, 10, 100, "Nome do curso deve ter entre 10 e 100 caracteres", validacao);
        ValidacaoNumerica.DeveSerMaiorQueZero(valor, "Valor do curso deve ser maior que zero", validacao);
        ValidacaoObjeto.DeveEstarInstanciado(conteudoProgramatico, "Conteúdo programático não foi informado", validacao);

        if (validoAte.HasValue)
        {
            ValidacaoData.DeveSerValido(validoAte.Value, "Data de validade do curso não foi informada", validacao);
        }

        validacao.DispararExcecaoDominioSeInvalido();
    }

    private void ValidarOrdemAula(byte ordemAula)
    {
        var validacao = new ResultadoValidacao<Curso>();

        if (_aulas.Any(a => a.OrdemAula == ordemAula))
        {
            validacao.AdicionarErro($"Já existe uma aula com a ordem {ordemAula}. Ordem da aula deve ser única dentro do curso");
        }

        validacao.DispararExcecaoDominioSeInvalido();
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        string cursoAtivo = Ativo ? "Sim" : "Não";
        return $"Curso {Nome} com valor de {Valor:N2} (Ativo? {cursoAtivo})";
    }
    #endregion
}
