using Stock.Domain.Entities;

namespace Stock.Domain.Specifications
{
    /// <summary>
    /// Represents a specification that retrieves all entities of a given type.
    /// This is useful for operations like counting all items.
    /// </summary>
    /// <typeparam name="T">The type of the entity.</typeparam>
    public class GetAllSpecification<T> : BaseSpecification<T> where T : BaseEntity
    {
        public GetAllSpecification() : base()
        {
        }
    }
} 