using Plataforma.Educacao.Core.Exceptions;

namespace Plataforma.Educacao.Core.Entities;
public abstract class Entidade
{
    #region Atributos
    public Guid Id { get; internal set; }
    #endregion

    #region Construtores
    protected Entidade()
    {
        Id = Guid.NewGuid();
    }
    #endregion

    #region Metodos do Dominio
    public void DefinirId(Guid id)
    {
        if (id == Guid.Empty) { throw new DomainException("Id não pode ser vazio"); }
        Id = id;
    }
    #endregion

    #region Validações  
    #endregion

    #region Overrides
    public override bool Equals(object obj)
    {
        var objeto = (obj as Entidade);

        if (ReferenceEquals(this, objeto)) return true;
        if (ReferenceEquals(null, objeto)) return false;

        return Id.Equals(objeto.Id);
    }

    public static bool operator ==(Entidade a, Entidade b)
    {
        if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
            return true;

        if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
            return false;

        return a.Equals(b);
    }

    public static bool operator !=(Entidade a, Entidade b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (GetType().GetHashCode() * (new Random().Next(1, int.MaxValue))) + Id.GetHashCode();
    }

    public override string ToString()
    {
        return $"{GetType().Name} [Id={Id}]";
    }

    //public virtual bool EhValido()
    //{
    //    throw new NotImplementedException();
    //}
    #endregion
}
