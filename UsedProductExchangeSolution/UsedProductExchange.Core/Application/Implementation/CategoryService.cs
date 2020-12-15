using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Filter;

namespace UsedProductExchange.Core.Application.Implementation
{
    public class CategoryService : IService<Category>
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository ?? throw new ArgumentException("Repository is missing");
        }

        private void ValidationCheck(Category category)
        {
            // Null or empty checks
            if (category == null)
            {
                throw new ArgumentException("Category is missing");
            }
            if (string.IsNullOrEmpty(category.Name))
            {
                throw new ArgumentException("Invalid category property: name");
            }
        }

        public FilteredList<Category> GetAll(Filter.Filter filter)
        {
            if (!string.IsNullOrEmpty(filter.SearchText) && string.IsNullOrEmpty(filter.SearchField))
            {
                filter.SearchField = "name";
            }
            return _categoryRepository.GetAll(filter);
        }

        public List<Category> GetAll()
        {
            return _categoryRepository.GetAll().ToList();
        }

        public Category Get(int id)
        {
            return _categoryRepository.Get(id);
        }

        public Category Add(Category entity)
        {
            ValidationCheck(entity);
            
            // Check if already existing
            if (_categoryRepository.Get(entity.CategoryId) != null)
            {
                throw new InvalidOperationException("Category already exists");
            }

            return _categoryRepository.Add(entity);
        }

        public Category Update(Category entity)
        {
            ValidationCheck(entity);

            if (entity == null || _categoryRepository.Get(entity.CategoryId) == null)
            {
                throw new InvalidOperationException("Category to update not found");
            }
            return _categoryRepository.Edit(entity);
        }

        public Category Delete(int id)
        {
            if (_categoryRepository.Get(id) == null)
            {
                throw new InvalidOperationException("Category not found");
            }
            
            return _categoryRepository.Remove(id);
        }
    }
}
