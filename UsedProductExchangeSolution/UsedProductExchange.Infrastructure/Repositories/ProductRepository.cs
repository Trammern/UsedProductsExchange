using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Infrastructure
{
    public class ProductRepository : IRepository<Product>
    {
        public Product Add(Product entity)
        {
            throw new NotImplementedException();
        }

        public Product Edit(Product entity)
        {
            throw new NotImplementedException();
        }

        public Product Get(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAll()
        {
            throw new NotImplementedException();
        }

        public Product Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
