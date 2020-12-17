using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Filter;

namespace UsedProductExchange.Core.Application.Implementation
{
    public class UserService : IService<User>
    {
        private readonly IRepository<User> _userRepository;
        
        public UserService(IRepository<User> userRepository)
        {
            _userRepository = userRepository ?? throw new ArgumentException("Repository is missing");
        }

        private void UserValidationCheck(User user)
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
        }

        private void AddUserValidationCheck(User user)
        {
            // Check if valid email
            var isEmail = Regex.IsMatch(user.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                throw new ArgumentException("Email is invalid");
            }

            // Check if username or email is taken 
            foreach (var users in _userRepository.GetAll())
            {
                if (user.Username == users.Username)
                {
                    throw new InvalidOperationException("Username is taken");
                }
                if (user.Email == users.Email)
                {
                    throw new InvalidOperationException("Email is taken");
                }
            }
        }
        
        public FilteredList<User> GetAll(Filter.Filter filter)
        {
            return _userRepository.GetAll(filter);
        }
        
        public List<User> GetAll()
        {
            return _userRepository.GetAll().ToList();
        }
        
        public User Get(int id)
        {
            return _userRepository.Get(id);
        }
        public User Add(User entity)
        {
            UserValidationCheck(entity);
            AddUserValidationCheck(entity);
            
            // Check if already existing
            if (_userRepository.Get(entity.UserId) != null)
            {
                throw new InvalidOperationException("User already exists");
            }
            
            return _userRepository.Add(entity);
        }
        
        public User Update(User entity)
        {
            UserValidationCheck(entity);

            if (entity == null || _userRepository.Get(entity.UserId) == null)
            {
                throw new InvalidOperationException("User to update not found");
            }

            return _userRepository.Edit(entity);
        }
        
        public User Delete(int id)
        {
            if (_userRepository.Get(id) == null)
            {
                throw new InvalidOperationException("User not found");
            }
            
            return _userRepository.Remove(id);
        }
    }
}
