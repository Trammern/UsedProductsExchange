using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Infrastructure.Context;

namespace UsedProductExchange.Infrastructure
{
    public class ProductRepository : IRepository<Product>
    {

        private readonly UsedProductExchangeContext _ctx;

        public ProductRepository(UsedProductExchangeContext ctx)
        {
            _ctx = ctx;
        }

        public Product Add(Product entity)
        {
            _ctx.Attach(entity).State = EntityState.Added;
            _ctx.SaveChanges();
            return entity;
        }

        public Product Edit(Product entity)
        {
            var productToUpdate = _ctx.Products.Update(entity);
            _ctx.SaveChanges();
            return productToUpdate.Entity;
        }

        public Product Get(int id)
        {
            return _ctx.Products.FirstOrDefault(p => p.ProductId == id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _ctx.Products.ToList();
        }

        public Product Remove(int id)
        {
            var productToDelete = Get(id);
            _ctx.Products.Remove(productToDelete);
            return productToDelete;
        }
    }
}
