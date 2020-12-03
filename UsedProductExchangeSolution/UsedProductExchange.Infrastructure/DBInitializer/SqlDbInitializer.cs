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
        }
    }
}