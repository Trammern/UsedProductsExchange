using UsedProductExchange.Core.Entities;

namespace UsedProductExchange.Core.Application
{
    public interface ILoginService
    {
        void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
        string GenerateToken(User user);
    }
}