using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Infrastructure.Context;

namespace UsedProductExchange.Infrastructure
{
    public class UserRepository : IRepository<User>
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
            return _ctx.Users.FirstOrDefault(u => u.UserId == id);
        }

        public IEnumerable<User> GetAll()
        {
            return _ctx.Users.ToList();
        }

        public User Remove(int id)
        {
            var deleteUser = _ctx.Users.Remove(Get(id));
            _ctx.SaveChanges();
            return deleteUser.Entity;
        }
    }
}
