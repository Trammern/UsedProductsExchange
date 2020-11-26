using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Application.Implementation
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _iUserRepository;
        
        public UserService(IUserRepository userRepository)
        {
            _iUserRepository = userRepository ?? throw new ArgumentException("Repository is missing");
        }

        public void UserValidationCheck(User user)
        {
            // Null or empty checks
            if (user == null)
            {
                throw new ArgumentException("User is missing");
            }
            if (String.IsNullOrEmpty(user.Name))
            {
                throw new ArgumentException("Invalid user property: name");
            }
            if (String.IsNullOrEmpty(user.Email))
            {
                throw new ArgumentException("Invalid user property: email");
            }
            if (string.IsNullOrEmpty(user.Address))
            {
                throw new ArgumentException("Invalid user property: address");
            }
            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ArgumentException("Invalid user property: username");
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Invalid user property: password");
            }
        }

        public User CreateUser(User user)
        {
            UserValidationCheck(user);
            
            // Check if already existing
            if (_iUserRepository.GetUserById(user.UserId) != null)
            {
                throw new InvalidOperationException("User already exists");
            }

            // Check if valid email
            bool isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                throw new ArgumentException("Email is invalid");
            }

            return _iUserRepository.CreateUser(user);
        }



        public User DeleteUser(int id)
        {
            if (_iUserRepository.GetUserById(id) == null)
            {
                throw new InvalidOperationException("User not found");
            }

            return _iUserRepository.DeleteUser(id);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _iUserRepository.GetAllUsers();
        }

        public User GetUserById(int id)
        {
            return _iUserRepository.GetUserById(id);
        }

        public User UpdateUser(User userToUpdate)
        {
            UserValidationCheck(userToUpdate);

            if (userToUpdate == null || _iUserRepository.GetUserById(userToUpdate.UserId) == null)
            {
                throw new InvalidOperationException("User to update not found");
            }
            return _iUserRepository.UpdateUser(userToUpdate);
        }
    }
}
