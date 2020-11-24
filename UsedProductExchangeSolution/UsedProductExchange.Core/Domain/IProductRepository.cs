using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Domain
{
    public interface IProductRepository
    {
        // CREATE
        public Product CreateProduct(Product product);

        // READ
        public IEnumerable<Product> GetAllProducts();
        public Product GetProductById(int id);


        // UPDATE
        public Product UpdateProduct(Product userToUpdate);


        // DELETE
        public Product DeleteProduct(Product product);

    }
}
