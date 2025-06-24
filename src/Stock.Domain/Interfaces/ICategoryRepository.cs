using Stock.Domain.Entities;

namespace Stock.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        // Kategoriye özgü ek metotlar buraya eklenebilir
    }
} 