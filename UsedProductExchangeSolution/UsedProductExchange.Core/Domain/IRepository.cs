using System.Collections.Generic;
using UsedProductExchange.Core.Filter;

namespace UsedProductExchange.Core.Domain
{
    public interface IRepository<T>
    {
        // Get all entities
        FilteredList<T> GetAll(Filter.Filter filter);
        IEnumerable<T> GetAll();
        // Get entity
        T Get(int id);
        // Add entity
        T Add(T entity);
        // Edit entity
        T Edit(T entity);
        // Remove Entity
        T Remove(int id);
    }
}