using Plataforma.Educacao.Core.Entities;
using Plataforma.Educacao.Core.Validations;
using System.Text.Json.Serialization;

namespace Plataforma.Educacao.Aluno.Domain.Entities
{
    public class Certificado : Entidade
    {
        #region Atributos
        public Guid MatriculaCursoId { get; private set; }
        public DateTime DataSolicitacao { get; private set; }
        public string PathCertificado { get; private set; }

        #region Helper only for EF Mapping
        [JsonIgnore]
        public MatriculaCurso MatriculaCurso { get; private set; }
        #endregion
        #endregion

        #region Construtores
        public Certificado(Guid matriculaCursoId, string pathCertificado)
        {
            MatriculaCursoId = matriculaCursoId;
            DataSolicitacao = DateTime.Now;
            PathCertificado = pathCertificado ?? string.Empty;

            ValidarIntegridadeCertificado();
        }
        #endregion

        #region Setters
        public void AtualizarPathCertificado(string path)
        {
            ValidarIntegridadeCertificado(novoPath: path ?? string.Empty);
            PathCertificado = path;
        }
        #endregion

        #region Validações
        private void ValidarIntegridadeCertificado(string novoPath = null)
        {
            var path = novoPath ?? PathCertificado;

            var validacao = new ResultadoValidacao<Certificado>();

            ValidacaoGuid.DeveSerValido(MatriculaCursoId, "Matrícula do curso deve ser informada", validacao);
            ValidacaoTexto.DevePossuirConteudo(path, "Path do certificado não pode ser nulo ou vazio", validacao);
            ValidacaoTexto.DevePossuirTamanho(path, 1, 1024, "Path do certificado deve ter no máximo 1024 caracteres", validacao);

            validacao.DispararExcecaoDominioSeInvalido();
        }
        #endregion

        #region Overrides
        public override string ToString()
        {
            return $"Certificado da matrícula {MatriculaCursoId}, gerado em {DataSolicitacao:dd/MM/yyyy}";
        }
        #endregion
    }
}
