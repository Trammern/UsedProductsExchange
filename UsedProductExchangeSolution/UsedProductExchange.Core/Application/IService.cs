using System.Collections.Generic;
using UsedProductExchange.Core.Filter;

namespace UsedProductExchange.Core.Application
{
    public interface IService<T>
    {
        public FilteredList<T> GetAll(Filter.Filter filter);
        public List<T> GetAll();
        public T Get(int id);
        public T Add(T entity);
        public T Update(T entity);
        public T Delete(int id);
    }
}