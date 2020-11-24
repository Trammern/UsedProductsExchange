using System;
using System.Collections.Generic;
using System.Text;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _iUserRepository;
        
        public UserService(IUserRepository userRepository)
        {
            _iUserRepository = userRepository;
        }


        public User CreateUser(User user)
        {
            return _iUserRepository.CreateUser(user);
        }

        public User DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<User> GetAllUsers()
        {
            throw new NotImplementedException();
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
