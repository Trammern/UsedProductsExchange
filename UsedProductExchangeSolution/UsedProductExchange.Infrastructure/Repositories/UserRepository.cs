using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Infrastructure
{
    public class UserRepository : IRepository<User>
    {
        public User Add(User entity)
        {
            throw new NotImplementedException();
        }

        public User Edit(User entity)
        {
            throw new NotImplementedException();
        }

        public User Get(int id)
        {
            return new List<User>()
            {
                new User()
                {
                    Name="Halfdan",
                    Address="Mjølners Alle 4",
                    Email="H@gmail.com",
                    Password="HR123",
                    Role=false,
                    UserId=1,
                    Username="HRname"
                    
                }
            };
        }

        public IEnumerable<User> GetAll()
        {
            throw new NotImplementedException();
        }

        public User Remove(int id)
        {
            throw new NotImplementedException();
        }
    }
}
