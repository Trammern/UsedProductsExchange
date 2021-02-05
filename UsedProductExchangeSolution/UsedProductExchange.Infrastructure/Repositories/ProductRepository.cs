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
            var filteredList = new FilteredList<Product>();

            filteredList.TotalCount = _ctx.Products.Count();
            filteredList.FilterUsed = filter;

            IEnumerable<Product> filtering = _ctx.Products
                .Include(c => c.Category)
                .Include(b => b.Bids)
                .ThenInclude(u => u.User);

            
            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                switch (filter.SearchField)
                {
                    case "name":
                        filtering = filtering.Where(p => p.Name.ToLower().Contains(filter.SearchText.ToLower()));
                        break;
                    case "category":
                        filtering = filtering.Where(p => p.Category.Name.ToLower().Contains(filter.SearchText.ToLower()));
                        break;
                }
            }

            if (!string.IsNullOrEmpty(filter.OrderDirection) && !string.IsNullOrEmpty(filter.OrderProperty))
            {
           
                var prop = typeof(Product).GetProperty(filter.OrderProperty);
                filtering = "ASC".Equals(filter.OrderDirection) ?
                    filtering.OrderBy(o => prop.GetValue(o, null)) :
                    filtering.OrderByDescending(o => prop.GetValue(o, null));

            }
            
            filteredList.List = filtering.ToList();
            filteredList.ResultsFound = filtering.Count();
            
            return filteredList;
        }

        public IEnumerable<Product> GetAll()
        {
            return _ctx.Products
                .Include(c => c.Category)
                .Include(b => b.Bids)
                .ThenInclude(u => u.User)
                .AsNoTracking();
        }

        public Product Get(int id)
        {
            var product = _ctx.Products
                .Include(c => c.Category)
                .Include(b => b.Bids)
                .ThenInclude(u => u.User)
                .Include(u => u.User)
                .AsNoTracking()
                .FirstOrDefault(p => p.ProductId == id);

            return product;
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
