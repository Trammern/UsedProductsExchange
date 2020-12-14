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
            
            Console.WriteLine("Hash: " + adminPassHash);
            Console.WriteLine("Salt: " + adminPassSalt);

            
            // Add some users
            var users = new List<User>
            {
                new User
                {
                    UserId = 3,
                    Name = "Tommy",
                    Username = "Admin",
                    PasswordHash = adminPassHash,
                    PasswordSalt = adminPassSalt,
                    IsAdmin = true,
                    Address = "Anotherstreet1",
                    Email = "tommy@hotmail.com",
                },

                   new User
                {
                    UserId = 2,
                    Name = "Tommy",
                    Username = "Admin",
                    PasswordHash = adminPassHash,
                    PasswordSalt = adminPassSalt,
                    IsAdmin = true,
                    Address = "Anotherstreet1",
                    Email = "tommy@hotmail.com",
                },
                new User
                {
                    UserId = 1,
                    Name = "Carl",
                    Username = "User",
                    PasswordHash = userPassHash,
                    PasswordSalt = userPassSalt,
                    IsAdmin = false,
                    Address = "Anotherstreet1",
                    Email = "tommy@hotmail.com",
                },
            };
            
            // Add some products
            var products = new List<Product>
            {
                new Product
                {
                    Category = new Category{Name = "Test Category"},
                    ProductId = 1,
                    Name = "Blikspand",
                    Description = "Lavet af ler",
                    PictureUrl = "URLISGONE.PNG",
                    CurrentPrice = 1000.00,
                    Expiration = DateTime.Now,
                    UserId = 1
                },
            };
            
            // Add some bids
            var bids = new List<Bid>
            {
                new Bid()
                {
                    BidId = 1,
                    UserId = 1,
                    ProductId = 1,
                    Price = 100,
                    CreatedAt = DateTime.Now
                },
                new Bid()
                {
                    BidId = 2,
                    UserId = 2,
                    ProductId = 1,
                    Price = 200,
                    CreatedAt = DateTime.Now.AddMinutes(5)
                }
            };

            var categories = new List<Category>();

            for (int i = 0; i < 100; i++)
            {
                categories.Add(new Category
                {
                    CategoryId = i,
                    Name = "Category " + i,
                });
            }

            context.Users.AddRange(users);
            context.Products.AddRange(products);
            context.Categories.AddRange(categories);
            context.Bids.AddRange(bids);
            context.SaveChanges();
        }
    }
}
