using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Application
{
    public interface IProductService
    {
        // CREATE
        public Product CreateProduct(Product product);


        // READ
        public IEnumerable<Product> GetAllProduct();

        public Product GetProductById(int id);


        // UPDATE
        public Product UpdateUser(Product productToUpdate);


        // DELETE
        public Product DeleteProduct(int id);

    }
}
