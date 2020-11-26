using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Infrastructure.Context;
using UsedProductExchange.Infrastructure.Repositories;

namespace UsedProductExchange.Infrastructure.DBInitializer
{
   public class DBInitializer
    {
        private readonly IRepository<UserRepository> _userRepository;
        private readonly IRepository<ProductRepository> _productRepository;
        private readonly IRepository<CategoryRepository> _categoryRepository;
        private readonly UsedProductExchangeContext _ctx;

        public DBInitializer(
            UsedProductExchangeContext ctx,
            IRepository<UserRepository> userRepository,
            IRepository<ProductRepository> productRepository,
            IRepository<CategoryRepository> categoryRepository
            )
        {

            _ctx = ctx;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;


        }
       
       public void InitData()
        {
            var user = new User
            {
                UserId = 1,
                Name = "Tommy",
                Username = "tommy",
                Password = "qwe123",
                Address = "Anotherstreet1",
                Email = "tommy@hotmail.com",
                Role = false
            };
            _ctx.SaveChanges();

            var product = new Product
            {
                CategoryId = 1,
                ProductId = 1,
                Name = "Blikspand",
                Description = "Lavet af ler",
                PictureURL = "URLISGONE.PNG",
                CurrentPrice = 1000.00,
                Expiration = DateTime.Now,
                UserId = 1
            };
            _ctx.SaveChanges();

            var category = new Category
            {
                CategoryId = 1,
                Name = "Rester"
            };
            _ctx.SaveChanges();
        }
    }
}
