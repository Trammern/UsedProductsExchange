using System;
using System.Collections.Generic;
using System.Text;

namespace UsedProductExchange.Core.Entities
{
    public class User
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsAdmin { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool Role { get; set; }

    }
}
