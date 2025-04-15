using Plataforma.Educacao.Aluno.Domain.Enumerators;
using Plataforma.Educacao.Aluno.Domain.ValueObjects;
using Plataforma.Educacao.Core.Entities;
using Plataforma.Educacao.Core.Exceptions;
using Plataforma.Educacao.Core.Validations;
using System.Text.Json.Serialization;

namespace Plataforma.Educacao.Aluno.Domain.Entities;
public class MatriculaCurso : Entidade
{
    #region Atributos        
    public Guid AlunoId { get; private set; }
    public Guid CursoId { get; private set; }
    public string NomeCurso { get; private set; }
    public decimal Valor { get; private set; }
    public DateTime DataMatricula { get; private set; }
    public DateTime? DataConclusao { get; private set; }
    public EstadoMatriculaCursoEnum EstadoMatricula { get; private set; }
    public Certificado Certificado { get; private set; }

    private readonly List<HistoricoAprendizado> _historicoAprendizado = [];
    public IReadOnlyCollection<HistoricoAprendizado> HistoricoAprendizado => _historicoAprendizado.AsReadOnly();

    #region Helper only for EF Mapping
    [JsonIgnore]
    public Aluno Aluno { get; private set; }
    #endregion
    #endregion

    #region Construtores   
    protected MatriculaCurso() { }
    public MatriculaCurso(Guid alunoId, Guid cursoId, string nomeCurso, decimal valor)
    {
        AlunoId = alunoId;
        CursoId = cursoId;
        NomeCurso = nomeCurso;
        Valor = valor;
        DataMatricula = DateTime.Now;
        EstadoMatricula = EstadoMatriculaCursoEnum.PendentePagamento;

        ValidarIntegridadeMatriculaCurso();
    }
    #endregion

    #region Getters
    public bool CursoConcluido => DataConclusao.HasValue;

    public HistoricoAprendizado ObterHistorico(Guid aulaId)
    {
        var historico = _historicoAprendizado.FirstOrDefault(h => h.AulaId == aulaId);
        if (historico == null) { throw new DomainException("Historico não foi localizado"); }

        return historico;
    }
    #endregion

    #region Metodos do Dominio
    public void AtualizarPagamentoMatricula()
    {
        ValidarIntegridadeMatriculaCurso(novoEstadoMatriculaCurso: EstadoMatriculaCursoEnum.PagamentoRealizado);
        EstadoMatricula = EstadoMatriculaCursoEnum.PagamentoRealizado;
    }

    public void AtualizarAbandonoMatricula()
    {
        ValidarIntegridadeMatriculaCurso(novoEstadoMatriculaCurso: EstadoMatriculaCursoEnum.Abandonado);
        EstadoMatricula = EstadoMatriculaCursoEnum.Abandonado;
    }

    public void ConcluirCurso()
    {
        var dataAtual = DateTime.Now;
        ValidarIntegridadeMatriculaCurso(novaDataConclusao: dataAtual);
        DataConclusao = dataAtual;

        // TODO :: Lançar evento de solicitação do certificado. Pendente de criação
    }

    public void RequisitarCertificadoConclusao(string pathCertificado)
    {
        if (Certificado != null) { throw new DomainException("Certificado já foi solicitado para esta matrícula."); }
        Certificado = new Certificado(Id, pathCertificado);
    }

    public void RegistrarHistoricoAprendizado(Guid aulaId, string nomeAula, DateTime? dataTermino = null)
    {
        var existente = _historicoAprendizado.FirstOrDefault(h => h.AulaId == aulaId);
        if (existente != null && existente.DataTermino.HasValue) { throw new DomainException("Esta aula já foi concluída"); }

        if (existente != null)
        {
            _historicoAprendizado.Remove(existente);
        }

        _historicoAprendizado.Add(new HistoricoAprendizado(CursoId, aulaId, nomeAula, dataTermino));
    }
    #endregion

    #region Validações
    private void ValidarIntegridadeMatriculaCurso(DateTime? novaDataConclusao = null, EstadoMatriculaCursoEnum? novoEstadoMatriculaCurso = null)
    {
        var dataConclusao = novaDataConclusao ?? DataConclusao;

        var validacao = new ResultadoValidacao<MatriculaCurso>();
        ValidacaoGuid.DeveSerValido(AlunoId, "Aluno deve ser informado", validacao);
        ValidacaoGuid.DeveSerValido(CursoId, "Curso deve ser informado", validacao);
        ValidacaoTexto.DevePossuirConteudo(NomeCurso, "Nome do curso deve ser informado", validacao);
        ValidacaoTexto.DevePossuirTamanho(NomeCurso, 10, 100, "Nome do curso deve ter entre 10 e 100 caracteres", validacao);
        ValidacaoNumerica.DeveSerMaiorQueZero(Valor, "Valor da matrícula deve ser maior que zero", validacao);

        ValidarConclusaoCurso(dataConclusao, validacao);
        ValidarEstadoParaAbandono(novoEstadoMatriculaCurso, dataConclusao, validacao);

        validacao.DispararExcecaoDominioSeInvalido();
    }

    private void ValidarConclusaoCurso(DateTime? dataConclusao, ResultadoValidacao<MatriculaCurso> validacao)
    {
        if (dataConclusao.HasValue)
        {
            switch (EstadoMatricula)
            {
                case EstadoMatriculaCursoEnum.PendentePagamento:
                case EstadoMatriculaCursoEnum.PagamentoRealizado:
                    ValidacaoData.DeveSerValido(dataConclusao.Value, "Data de conclusão deve ser informada", validacao);
                    ValidacaoData.DeveTerRangeValido(DataMatricula, dataConclusao.Value, "Data de conclusão não pode ser anterior a data de matrícula", validacao);
                    break;
                case EstadoMatriculaCursoEnum.Abandonado:
                    validacao.AdicionarErro("Não é possível concluir um curso com estado de pagamento abandonado");
                    break;
            }
        }
    }

    private void ValidarEstadoParaAbandono(EstadoMatriculaCursoEnum? novoEstado, DateTime? dataConclusao, ResultadoValidacao<MatriculaCurso> validacao)
    {
        if (novoEstado.HasValue && novoEstado == EstadoMatriculaCursoEnum.Abandonado && dataConclusao.HasValue)
        {
            validacao.AdicionarErro("Não é possível alterar o estado da matrícula para pagamento abandonado com o curso concluído");
        }
    }
    #endregion

    #region Overrides
    public override string ToString()
    {
        string concluido = CursoConcluido ? "Sim" : "Não";
        return $"Matrícula no curso {CursoId} do aluno {AlunoId} (Concluído? {concluido})";
    }
    #endregion
}
