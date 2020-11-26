using System.Collections.Generic;

namespace UsedProductExchange.Core.Domain
{
    public interface IRepository<T>
    {
        // Get all entities
        IEnumerable<T> GetAll();
        // Get entity
        T Get(long id);
        // Add entity
        T Add(T entity);
        // Edit entity
        T Edit(T entity);
        // Remove Entity
        T Remove(long id);
    }
}