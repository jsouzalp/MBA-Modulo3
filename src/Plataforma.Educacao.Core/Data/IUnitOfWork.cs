namespace Plataforma.Educacao.Core.Data
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
