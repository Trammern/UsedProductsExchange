using System.Collections.Generic;
using UsedProductExchange.Infrastructure.Context;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Infrastructure.DBInitializer
{
    public class SqlDbInitializer: IDbInitializer
    {
        private readonly ILoginService _loginService;
        
        public SqlDbInitializer(ILoginService loginService)
        {
            _loginService = loginService;
        }
        
        public void Initialize(UsedProductExchangeContext context)
        {
            // Create the database, if it does not already exists.
            context.Database.EnsureCreated();

            // Check if there is any Pets, Owners or Users in the database
            if (context.Users.Any() || context.Products.Any() || context.Categories.Any() || context.Bids.Any())
            {
                // Make sure the tables are dropped
                context.Database.ExecuteSqlRaw("DROP TABLE Users");
                context.Database.ExecuteSqlRaw("DROP TABLE Products");
                context.Database.ExecuteSqlRaw("DROP TABLE Categories");
                context.Database.ExecuteSqlRaw("DROP TABLE Bids");
                // Re-create the database
                context.Database.EnsureCreated();
            }
            
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
                }
            };
            
            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}