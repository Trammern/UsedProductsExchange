using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Infrastructure
{
    public class CategoryRepository : IRepository<Category>
    {
        public IEnumerable<Category> GetAll()
        {
            throw new NotImplementedException();
        }

        public Category Get(int id)
        {
            throw new NotImplementedException();
        }

        public Category Add(Category entity)
        {
            throw new NotImplementedException();
        }

        public Category Edit(Category entity)
        {
            throw new NotImplementedException();
        }

        public Category Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
