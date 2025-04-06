namespace Plataforma.Educacao.Core.Entities
{
    public abstract class Entidade
    {
        #region Atributos
        public Guid Id { get; set; }
        #endregion

        #region Construtores
        protected Entidade()
        {
            Id = Guid.NewGuid();
        }
        #endregion

        #region Setters
        #endregion

        #region Validações  
        #endregion

        #region Overrides
        //public override bool Equals(object obj)
        //{
        //    var objetoParaComparar = (obj as Entidade);

        //    if (ReferenceEquals(this, objetoParaComparar)) return true;
        //    if (ReferenceEquals(null, objetoParaComparar)) return false;

        //    return Id.Equals(objetoParaComparar.Id);
        //}

        //public static bool operator ==(Entidade a, Entidade b)
        //{
        //    if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
        //        return true;

        //    if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
        //        return false;

        //    return a.Equals(b);
        //}

        //public static bool operator !=(Entidade a, Entidade b)
        //{
        //    return !(a == b);
        //}

        //public override int GetHashCode()
        //{
        //    return (GetType().GetHashCode() * (new Random().Next(1, int.MaxValue))) + Id.GetHashCode();
        //}

        //public override string ToString()
        //{
        //    return $"{GetType().Name} [Id={Id}]";
        //}

        //public virtual bool EhValido()
        //{
        //    throw new NotImplementedException();
        //}
        #endregion

    }
}
