using Moq;
using UsedProductExchange.Core.Application;
using UsedProductExchange.Core.Application.Implementation;
using UsedProductExchange.Core.Entities;
using Xunit;

namespace UsedProductExchange.XUnitTestProject
{
    public class LoginServiceTest
    {
        private readonly ILoginService _loginService;

        public LoginServiceTest()
        {
            var secretBytes = new byte[40];
            _loginService = new LoginService(secretBytes);
        }
        
        #region Login

        [Theory]
        [InlineData(1, "Jimmy", "Jimster", "qaz123", "Storegade 1", "jimster@hotmail.com", true)]
        [InlineData(2, "Carl", "Madsen", "password", "Storegade 1", "carlm@hotmail.com", true)]
        [InlineData(3, "Hans", "Kristensen", "hGytrW96lopF", "Storegade 1", "hanskris22@hotmail.com", true)]
        [InlineData(4, "Emma", "Mogens", "emmamogens123", "Storegade 1", "emmamogens@hotmail.com", true)]
        public void UserPasswordsAreCanBeHashedAndVerified(int id, string name, string username, string password, string address, string email, bool isAdmin)
        {
            _loginService.CreatePasswordHash(password, out var passHash, out var passSalt);

            var user = new User
            {
                UserId = id,
                Name = name,
                Username = username,
                PasswordHash = passHash,
                PasswordSalt = passSalt,
                IsAdmin = isAdmin,
                Address = address,
                Email = email,
            };

            Assert.True(_loginService.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt));
        }

        #endregion
    }
}