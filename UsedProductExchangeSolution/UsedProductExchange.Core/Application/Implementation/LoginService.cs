using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Application.Implementation
{
    public class LoginService: ILoginService
    {
        private readonly byte[] _secretBytes;

        public LoginService(byte[] secret)
        {
            _secretBytes = secret;
        }
        
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return !computedHash.Where((t, i) => t != storedHash[i]).Any();
        }

        public string GenerateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            if (user.IsAdmin)
                claims.Add(new Claim(ClaimTypes.Role, "Administrator"));

            var token = new JwtSecurityToken(
                new JwtHeader(new SigningCredentials(
                    new SymmetricSecurityKey(_secretBytes),
                    SecurityAlgorithms.HmacSha256)
                ),
                new JwtPayload(null,    // issuer - not needed (ValidateIssuer = false)
                    null, // audience - not needed (ValidateAudience = false)
                    claims.ToArray(),
                    DateTime.Now,              // notBefore
                    DateTime.Now.AddMinutes(60*24*7) // expires
                )
            );  

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}