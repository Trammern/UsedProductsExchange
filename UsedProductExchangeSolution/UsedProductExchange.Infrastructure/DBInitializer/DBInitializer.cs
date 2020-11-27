using System;
using System.Collections.Generic;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Infrastructure.Context;
using UsedProductExchange.Infrastructure.Repositories;

namespace UsedProductExchange.Infrastructure.DBInitializer
{
    public class DbInitializer: IDbInitializer
    {
        private readonly ILoginService _loginService;

        public DbInitializer(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public void Initialize(UsedProductExchangeContext context)
        {
            // Delete the database, if it already exists.
            context.Database.EnsureDeleted();

            // Create the database, if it does not already exists.
            context.Database.EnsureCreated();
            
            // Create two users with hashed and salted passwords
            const string password = "passw0rd";
            _loginService.CreatePasswordHash(password, out var adminPassHash, out var adminPassSalt);
            _loginService.CreatePasswordHash(password, out var userPassHash, out var userPassSalt);

            
            // Add some users
            var users = new List<User>
            {
                new User
                {
                    UserId = 1,
                    Name = "Tommy",
                    Username = "Admin",
                    PasswordHash = adminPassHash,
                    PasswordSalt = adminPassSalt,
                    IsAdmin = true,
                    Address = "Anotherstreet1",
                    Email = "tommy@hotmail.com",
                    Role = false
                },
                new User
                {
                    UserId = 2,
                    Name = "Carl",
                    Username = "User",
                    PasswordHash = userPassHash,
                    PasswordSalt = userPassSalt,
                    IsAdmin = false,
                    Address = "Anotherstreet1",
                    Email = "tommy@hotmail.com",
                    Role = false
                },
            };
            
            // Add some users
            var products = new List<Product>
            {
                new Product
                {
                    CategoryId = 1,
                    ProductId = 1,
                    Name = "Blikspand",
                    Description = "Lavet af ler",
                    PictureUrl = "URLISGONE.PNG",
                    CurrentPrice = 1000.00,
                    Expiration = DateTime.Now,
                    UserId = 1
                },
            };

            var categories = new List<Category>
            {
                new Category
                {
                    CategoryId = 1,
                    Name = "Rester"
                },
            };
            
            context.Users.AddRange(users);
            context.Products.AddRange(products);
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }
    }
}
