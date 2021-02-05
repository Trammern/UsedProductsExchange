using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Infrastructure.Context
{
    public class UsedProductExchangeContext : DbContext
    {

        // Calling super class constructor
        public UsedProductExchangeContext(DbContextOptions<UsedProductExchangeContext> opt) : base(opt) { }

        public DbSet<User> Users { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Bid> Bids { get; set; }


    }
}
