using System;
using Xunit;
using Moq;
using UsedProductExchange.Core.Domain;
using System.Collections.Generic;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Application.Implementation;

namespace UsedProductExchange.XUnitTestProject
{
    public class UserServiceTest
    {
        private List<User> users = null;
        private readonly Mock<IUserRepository> repoMock;

        public UserServiceTest()
        {
            repoMock = new Mock<IUserRepository>();
            repoMock.Setup(repo => repo.GetAllUsers()).Returns(() => users);
        }

        [Theory]
        [InlineData(1, "Jimmy", "jimster", "qaz123", "Storegade 1", "jimster@hotmail.com", true)]
        public void TestIfNewsUserIsCreated(int id, string name, string username, string password, string address, string email, bool role)
        {
            // ARRANGE
            User user = new User()
            {
                UserId = id,
                Name = name,
                Username = username,
                Password = password,
                Address = address,
                Email = email,
                Role = role
            };
            UserService us = new UserService(repoMock.Object);

            // ACT
            var newUser = us.CreateUser(user);

            // ASSERT
            repoMock.Verify(repo => repo.CreateUser(user), Times.Once);

        }
    }
}
