using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Application.Implementation
{
    public class ProductService : IProductService
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


        public Product CreateProduct(Product product)
        {
            ProductValidationCheck(product);

            // Check if already existing
            if (_iProductRepository.Get(product.ProductId) != null)
            {
                throw new InvalidOperationException("Product already exists");
            }
            return _iProductRepository.Add(product);
        }

        public Product DeleteProduct(Product product)
        {
            return _iProductRepository.Remove(product.ProductId);
        }

        public IEnumerable<Product> GetAllProduct()
        {
            return _iProductRepository.GetAll();
        }

        public Product GetProductById(int id)
        {
            return _iProductRepository.Get(id);
        }

        public Product UpdateProduct(Product productToUpdate)
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
