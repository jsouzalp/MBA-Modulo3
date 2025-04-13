using Plataforma.Educacao.Core.Aggregates;

namespace Plataforma.Educacao.Core.Data;
public interface IRepository<T> : IDisposable where T : IRaizAgregacao
{
    IUnitOfWork UnitOfWork { get; }
}
