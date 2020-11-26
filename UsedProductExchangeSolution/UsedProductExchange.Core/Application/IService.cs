using System.Collections.Generic;

namespace UsedProductExchange.Core.Application
{
    public interface IService<T>
    {
        public List<T> GetAll();
        public T Get(int id);
        public T Add(T entity);
        public T Update(T entity);
        public T Delete(int id);
    }
}