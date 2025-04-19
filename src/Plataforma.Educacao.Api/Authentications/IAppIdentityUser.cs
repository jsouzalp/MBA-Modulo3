namespace Plataforma.Educacao.Api.Authentications;
public interface IAppIdentityUser
{
    Guid ObterUsuarioId();
    bool EstahAutenticado();
    bool EhAdministrador();
    string ObterEmail();
}