using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Application.Implementation
{
    public class ProductService : IService<Product>
    {
        private readonly IRepository<Product> _productRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _productRepository = productRepository ?? throw new ArgumentException("Repository is missing");
        }

        private void ProductValidationCheck(Product product)
        {
            // Null or empty checks
            if (product == null)
            {
                throw new ArgumentException("Product is missing");
            }
            if (String.IsNullOrEmpty(product.Name))
            {
                throw new ArgumentException("Invalid product property: name");
            }
            if (String.IsNullOrEmpty(product.Description))
            {
                throw new ArgumentException("Invalid product property: description");
            }
            if (string.IsNullOrEmpty(product.PictureURL))
            {
                throw new ArgumentException("Invalid product property: Picture");
            }
        }

        public Product Add(Product entity)
        {
            ProductValidationCheck(entity);

            // Check if already existing
            if (_productRepository.Get(entity.ProductId) != null)
            {
                throw new InvalidOperationException("Product already exists");
            }
            return _productRepository.Add(entity);
        }

        public Product Delete(int id)
        {
            return _productRepository.Remove(id);
        }

        public List<Product> GetAll()
        {
            return _productRepository.GetAll().ToList();
        }

        public Product Get(int id)
        {
            return _productRepository.Get(id);
        }

        public Product Update(Product entity)
        {
            if (entity.Name == null)
            {
                throw new InvalidOperationException("Product must have a name");
            }
            else
            {
                var updatedProduct = _productRepository.Edit(entity);
                return updatedProduct;
            }
        }
    }
}
