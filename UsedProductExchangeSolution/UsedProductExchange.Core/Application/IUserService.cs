using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Application
{
    public interface IUserService
    {
        // CREATE
        public User CreateUser(User user);


        // READ
        public IEnumerable<User> GetAllUsers();

        public User GetUserById(int id);


        // UPDATE
        public User UpdateUser(User userToUpdate);


        // DELETE
        public User DeleteUser(int id);


    }
}
