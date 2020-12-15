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
    public class UserRepository: IRepository<User>
    {
        private readonly UsedProductExchangeContext _ctx;

        public UserRepository(UsedProductExchangeContext ctx)
        {
            _ctx = ctx;
        }

        public User Add(User entity)
        {
            var newUser = _ctx.Users.Add(entity);
            _ctx.SaveChanges();
            return newUser.Entity;
        }

        public User Edit(User entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            _ctx.SaveChanges();
            return entity;
        }

        public User Get(int id)
        {
            return _ctx.Users
                .Include(u => u.Products)
                .Include(us => us.Bids)
                .AsNoTracking()
                .FirstOrDefault(use => use.UserId == id);
        }

        public FilteredList<User> GetAll(Filter filter)
        {
            var filteredList = new FilteredList<User>
            {
                TotalCount = _ctx.Users.Count(),
                FilterUsed = filter,
                List = _ctx.Users.Select(u => new User()
                    {
                        UserId = u.UserId, 
                        Name = u.Name,
                        Username = u.Username,
                        Address = u.Address,
                        Email = u.Email,
                        IsAdmin = u.IsAdmin
                    })
                    .ToList()
            };
            return filteredList;
        }
        
        public IEnumerable<User> GetAll()
        {
            return _ctx.Users;
        }

        public User Remove(int id)
        {
            var user = _ctx.Users.FirstOrDefault(x => x.UserId == id);
            if(user == null) throw new ArgumentException("User does not exist");
            var deletedUser = _ctx.Users.Remove(user);
            _ctx.SaveChanges();
            return deletedUser.Entity;
        }
    }
}
