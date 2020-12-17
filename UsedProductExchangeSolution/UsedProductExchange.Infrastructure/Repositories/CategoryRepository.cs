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
    public class CategoryRepository : IRepository<Category>
    {
        private readonly UsedProductExchangeContext _ctx;
        private Mail mail;

        public CategoryRepository(UsedProductExchangeContext ctx)
        {
            _ctx = ctx;
            mail = new Mail();
        }
        
        public FilteredList<Category> GetAll(Filter filter)
        {
            var filteredList = new FilteredList<Category>();

            filteredList.TotalCount = _ctx.Categories.Count();
            filteredList.FilterUsed = filter;

            IEnumerable<Category> filtering = _ctx.Categories
                .Include(p => p.Products);

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                switch (filter.SearchField)
                {
                    case "name":
                        filtering = filtering.Where(p => p.Name.ToLower().Contains(filter.SearchText.ToLower()));
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

        public IEnumerable<Category> GetAll()
        {
            return _ctx.Categories.Include(p => p.Products).AsNoTracking();
        }

        public Category Get(int id)
        {
            return _ctx.Categories
                .Include(p => p.Products)
                .ThenInclude(b => b.Bids)
                .ThenInclude(u => u.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.CategoryId == id);
        }

        public Category Add(Category entity)
        {
            mail.SendSimpleMessage("andreasbendorff@gmail.com", "New Category Created", "There were a new category added to the website.");
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
            if(category == null) throw new ArgumentException("Category does not exist");
            var deletedCategory = _ctx.Categories.Remove(category);
            _ctx.SaveChanges();
            return deletedCategory.Entity;
        }
    }
}
