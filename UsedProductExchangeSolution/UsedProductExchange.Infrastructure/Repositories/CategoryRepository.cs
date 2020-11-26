using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Infrastructure.Context;

namespace UsedProductExchange.Infrastructure.Repositories
{
    public class CategoryRepository : IRepository<Category>
    {
        private readonly UsedProductExchangeContext _ctx;

        public CategoryRepository(UsedProductExchangeContext ctx)
        {
            _ctx = ctx;
        }
        
        public IEnumerable<Category> GetAll()
        {
            return _ctx.Categories.ToList();
        }

        public Category Get(int id)
        {
            return _ctx.Categories.FirstOrDefault(x => x.CategoryId == id);
        }

        public Category Add(Category entity)
        {
            var category = _ctx.Categories.Add(entity);
            _ctx.SaveChanges();
            return category.Entity;
        }

        public Category Edit(Category entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            _ctx.SaveChanges();
            return entity;
        }

        public Category Remove(int id)
        {
            var category = _ctx.Categories.FirstOrDefault(x => x.CategoryId == id);
            if(category == null) throw new ArgumentException("Category is missing");
            _ctx.Categories.Remove(category);
            _ctx.SaveChanges();
            return category;
        }
    }
}
