using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Application.Implementation
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _iProductRepository;

        public ProductService(IProductRepository productRepository)
        {
            _iProductRepository = productRepository;
        }
        public Product CreateProduct(Product product)
        {
            return _iProductRepository.CreateProduct(product);
        }

        public Product DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetAllProduct()
        {
            throw new NotImplementedException();
        }

        public Product GetProductById(int id)
        {
            throw new NotImplementedException();
        }

        public Product UpdateUser(Product productToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
