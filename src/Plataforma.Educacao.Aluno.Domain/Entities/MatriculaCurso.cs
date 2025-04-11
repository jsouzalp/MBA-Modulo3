using Plataforma.Educacao.Aluno.Domain.Enumerators;
using Plataforma.Educacao.Core.Entities;
using Plataforma.Educacao.Core.Validations;
using System.Text.Json.Serialization;

namespace Plataforma.Educacao.Aluno.Domain.Entities
{
    public class MatriculaCurso : Entidade
    {
        #region Atributos        
        public Guid AlunoId { get; private set; }
        public Guid CursoId { get; private set; }
        public DateTime DataMatricula { get; private set; }
        public DateTime? DataConclusao { get; private set; }
        public EstadoMatriculaCursoEnum EstadoMatricula { get; private set; }

        #region Helper only for EF Mapping
        public Certificado Certificado { get; private set; }
        
        [JsonIgnore]
        public Aluno Aluno { get; private set; }
        #endregion
        #endregion

        #region Construtores        
        public MatriculaCurso(Guid alunoId, Guid cursoId)
        {
            AlunoId = alunoId;
            CursoId = cursoId;
            DataMatricula = DateTime.Now;
            EstadoMatricula = EstadoMatriculaCursoEnum.PendentePagamento;

            ValidarIntegridadeMatriculaCurso();
        }
        #endregion

        #region Getters
        public bool CursoConcluido => DataConclusao.HasValue;
        #endregion

        #region Setters
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
        }
        #endregion

        #region Validações
        private void ValidarIntegridadeMatriculaCurso(DateTime? novaDataConclusao = null, EstadoMatriculaCursoEnum? novoEstadoMatriculaCurso = null)
        {
            var dataConclusao = novaDataConclusao ?? DataConclusao;

            var validacao = new ResultadoValidacao<MatriculaCurso>();
            ValidacaoGuid.DeveSerValido(AlunoId, "Aluno deve ser informado", validacao);
            ValidacaoGuid.DeveSerValido(CursoId, "Curso deve ser informado", validacao);

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
}
