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
        private readonly IRepository<Product> _iProductRepository;

        public ProductService(IRepository<Product> productRepository)
        {
            _iProductRepository = productRepository ?? throw new ArgumentException("Repository is missing");
        }
      

        public void ProductValidationCheck(Product product)
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


        public Product Add(Product product)
        {
            ProductValidationCheck(product);

            // Check if already existing
            if (_iProductRepository.Get(product.ProductId) != null)
            {
                throw new InvalidOperationException("Product already exists");
            }
            return _iProductRepository.Add(product);
        }

        public Product Delete(int i)
        {
            return _iProductRepository.Remove(i);
        }

        public List<Product> GetAll()
        {
            return _iProductRepository.GetAll().ToList();
        }

        public Product Get(int id)
        {
            return _iProductRepository.Get(id);
        }

        public Product Update(Product productToUpdate)
        {
            
            if (productToUpdate.Name == null)
            {
                throw new InvalidOperationException("Product must have a name");
            }
            else
            {
                var updatedProduct = _iProductRepository.Edit(productToUpdate);
                return updatedProduct;
            }
        }
    }
}
