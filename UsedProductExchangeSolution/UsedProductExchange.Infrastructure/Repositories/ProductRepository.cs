using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Filter;
using UsedProductExchange.Infrastructure.Context;

namespace UsedProductExchange.Infrastructure.Repositories
{
    public class ProductRepository: IRepository<Product>
    {
        private readonly UsedProductExchangeContext _ctx;

        public ProductRepository(UsedProductExchangeContext ctx)
        {
            _ctx = ctx;
        }
        
        public FilteredList<Product> GetAll(Filter filter)
        {
            var filteredList = new FilteredList<Product>
            {
                TotalCount = _ctx.Products.Count(),
                FilterUsed = filter,
                List = _ctx.Products.Select(p => new Product()
                    {
                        ProductId = p.ProductId, 
                        UserId = p.UserId,
                        Name = p.Name,
                        Description = p.Description,
                        PictureUrl = p.PictureUrl,
                        CurrentPrice = p.CurrentPrice,
                        Expiration = p.Expiration,
                        Category = p.Category
                    })
                    .ToList()
            };
            return filteredList;
        }

        public IEnumerable<Product> GetAll()
        {
            return _ctx.Products;
        }

        public Product Get(int id)
        {
            return _ctx.Products.Include(c => c.Category).FirstOrDefault(p => p.ProductId == id);
        }

        public Product Add(Product entity)
        {
            var newProduct = _ctx.Products.Add(entity);
            _ctx.SaveChanges();
            return newProduct.Entity;
        }

        public Product Edit(Product entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            _ctx.SaveChanges();
            return entity;
        }

        public Product Remove(int id)
        {
            var product = _ctx.Products.FirstOrDefault(x => x.ProductId == id);
            if(product == null) throw new ArgumentException("Product does not exist");
            var deletedProduct = _ctx.Products.Remove(product);
            _ctx.SaveChanges();
            return deletedProduct.Entity;
        }
    }
}
