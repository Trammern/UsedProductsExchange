using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Infrastructure
{
    public class UserRepository : IUserRepository
    {
        public User CreateUser(User user)
        {
            throw new NotImplementedException();
        }

        public User DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
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

        public User GetUserById(int id)
        {
            throw new NotImplementedException();
        }

        public User UpdateUser(User userToUpdate)
        {
            throw new NotImplementedException();
        }
    }
}
