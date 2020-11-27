using System;
using Xunit;
using Moq;
using UsedProductExchange.Core.Domain;
using System.Collections.Generic;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Application.Implementation;
using FluentAssertions;

namespace UsedProductExchange.XUnitTestProject
{
    public class UserServiceTest
    {
        
        private List<User> _users = null;
        private readonly Mock<IRepository<User>> _repoMock;

        public UserServiceTest()
        {
            _repoMock = new Mock<IRepository<User>>();
            _repoMock.Setup(repo => repo.GetAll()).Returns(() => _users);
        }

        #region ServiceTest
        [Fact]
        public void CreateUserServiceWithRepository()
        {
            // ARRANGE
            var repo = _repoMock.Object;

            // ACT
            var service = new UserService(repo);

            // ASSERT
            Assert.NotNull(service);
        }

        [Fact]
        public void CreateUserService_InvalidRepository()
        {
            // ARRANGE
            UserService service = null;

            // ACT
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                service = new UserService(null);
            });

            // ASSERT
            Assert.Equal("Repository is missing", ex.Message);
            Assert.Null(service);
        }
        #endregion


        #region AddUserTest
        [Theory]
        [InlineData(1, "Jimmy", "jimster", "qaz123", "Storegade 1", "jimster@hotmail.com", true)]
        public void TestIfNewUserIsCreated_ValidInput(int id, string name, string username, string password, string address, string email, bool role)
        {
            // ARRANGE
            var user = new User()
            {
                UserId = id,
                Name = name,
                Username = username,
                Address = address,
                Email = email,
                Role = role
            };
            UserService us = new UserService(_repoMock.Object);

            var userList = new List<User>();

            // ACT
            var newUser = us.Add(user);
            userList.Add(user);
            

            // ASSERT
            _repoMock.Setup(u => u.Add(user)).Returns(newUser);
            _repoMock.Verify(repo => repo.Add(user), Times.Once);
            userList.Should().Contain(user);
        }

        [Theory]
        [InlineData(1, "", "billy", "qaz123", "Somestreet 1", "billy@hotmail.com", true, "name")] // Name is empty
        [InlineData(2, null, "joey", "qaz123", "Somestreet 2", "joey@hotmail.com", true, "name")] // Name is null
        [InlineData(3, "Joey", "joey", "qaz123", "Somestreet 2", "", true, "email")] // Email is empty
        [InlineData(4, "Joey", "joey", "qaz123", "Somestreet 2", null, true, "email")] // Email is null
        [InlineData(5, "Joey", "joey", "qaz123", "", "joey@hotmail.com", true, "address")] // Address is empty
        [InlineData(6, "Joey", "joey", "qaz123", null, "joey@hotmail.com", true, "address")] // Address is null
        [InlineData(7, "Joey", "", "qaz123", "Somestreet 3", "joey@hotmail.com", true, "username")] // Username is empty
        [InlineData(8, "Joey", null, "qaz123", "Somestreet 3", "joey@hotmail.com", true, "username")] // Username is null
        public void CreateNewUserWithInvalidInput_ExpectArgumentException(int id, string name, string username, string password, string address, string email, bool role, string errorField)
        {
            // ARRANGE
            var user = new User()
            {
                UserId = id,
                Name = name,
                Username = username,
                Address = address,
                Email = email,
                Role = role
            };

            UserService us = new UserService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<ArgumentException>(() => us.Add(user));

            // ASSERT
            Assert.Equal($"Invalid user property: {errorField}", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<User>(u => u == user)), Times.Never);
        }

        [Fact]
        public void AddUserWhoExists_ExpectInvalidArgumentException()
        {
            // ARRANGE
            var user = new User()
            {
                UserId = 1,
                Name = "Tommy",
                Username = "tommy",
                Address = "Anotherstreet1",
                Email = "tommy@hotmail.com",
                Role = false
            };

            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == user.UserId))).Returns(() => user);

            UserService us = new UserService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => us.Add(user));

            // ASSERT
            Assert.Equal("User already exists", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<User>(u => u == user)), Times.Never);
        }


        [Theory]
        [InlineData(1, "Jimmy", "jimster", "qaz123", "Storegade 1", "jimmys mail", true)]
        public void TestInvalidEmail(int id, string name, string username, string password, string address, string email, bool role)
        {
            // ARRANGE
            var user = new User()
            {
                UserId = id,
                Name = name,
                Username = username,
                Address = address,
                Email = email,
                Role = role
            };
            UserService us = new UserService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<ArgumentException>(() => us.Add(user));

            // ASSERT
            Assert.Equal("Email is invalid", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<User>(u => u == user)), Times.Never);

        }

        #endregion


        #region DeleteUserTest

        [Fact]
        public void RemoveExistingUser()
        {
            // ARRANGE
            var user = new User()
            {
                UserId = 1,
                Name = "Jack",
                Address = "Somestreet 1",
                Email = "jack@hotmail.com",
                Username = "jackster",
                Role = true
            };

            UserService us = new UserService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(u => u == user.UserId))).Returns(() => user);

            // ACT
            var deletedUser = us.Delete(user.UserId);

            // ASSERT
            _repoMock.Verify(repo => repo.Remove(It.Is<int>(u => u == user.UserId)), Times.Once);
            deletedUser.Should().BeNull();
        }

        [Fact]
        public void DeleteUserNotFound_ExpectInvalidOperationException()
        {
            // ARRANGE
            var user = new User()
            {
                UserId = 1,
                Name = "Jack",
                Address = "Somestreet 1",
                Email = "jack@hotmail.com",
                Username = "jackster",
                Role = true
            };

            UserService us = new UserService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => us.Delete(user.UserId));

            // ASSERT
            Assert.Equal("User not found", ex.Message);
            _repoMock.Verify(repo => repo.Remove(It.Is<int>(u => u == user.UserId)), Times.Never);
        }

        #endregion


        #region UpdateUserTest

        [Theory]
        [InlineData(1, "some name", "some username", "some password", "some address", "some email", false)]
        [InlineData(1, "some name", "some username", "some password", "some address", "updated email", false)]
        public void UpdateValidUser(int id, string name, string username, string password, string address, string email, bool role)
        {
            // ARRANGE
            var user = new User()
            {
                UserId = id,
                Name = name,
                Username = username,
                Address = address,
                Email = email,
                Role = role
            };

            UserService us = new UserService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(z => z == user.UserId))).Returns(() => user);

            // ACT
            var updatedUser = us.Update(user);

            // ASSERT
            _repoMock.Verify(repo => repo.Edit(It.Is<User>(u => u == user)), Times.Once);
        }

        [Fact]
        public void UpdatingUserNotFound_ExpectInvalidOperationException()
        {
            // ARRANGE
            var user = new User()
            {
                UserId = 1,
                Name = "Tommy",
                Username = "tommy",
                Address = "Anotherstreet1",
                Email = "tommy@hotmail.com",
                Role = false
            };

            UserService us = new UserService(_repoMock.Object);

            // check if not existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == user.UserId))).Returns(() => null);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => us.Update(user));

            // ASSERT
            Assert.Equal("User to update not found", ex.Message);
            _repoMock.Verify(repo => repo.Edit(It.Is<User>(u => u == user)), Times.Never);

        }

        [Fact]
        public void UpdatedUserShouldReplaceOldUser()
        {
            // ARRANGE
            var user = new User()
            {
                UserId = 1,
                Name = "Tommy",
                Username = "tommy",
                Address = "Anotherstreet1",
                Email = "tommy@hotmail.com",
                Role = false
            };
            UserService us = new UserService(_repoMock.Object);

            // check if user or id is not null
            _repoMock.Setup(u => u.Get(It.Is<int>(id => id == user.UserId))).Returns(() => user);
            _repoMock.Setup(u => u.Edit(user)).Returns(user);

            // ACT
            var updatedAddress = us.Update(user);

            // ASSERT (Fluent)
            updatedAddress.Should().Be(user);
            _repoMock.Verify(repo => repo.Edit(It.Is<User>(u => u == user)), Times.Once);
        }


        #endregion


        #region ReadUserTest

        [Fact]
        public void TestGetExistingUserById()
        {
            // ARRANGE
            var user = new User()
            {
                UserId = 1,
                Name = "name",
                Address = "address",
                Email = "email",
                Username = "username",
                Role = true
            };
            UserService us = new UserService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == user.UserId))).Returns(() => user);

            // ACT
            var userFound = us.Get(1);

            // ASSERT
            Assert.Equal(user, userFound);
            _repoMock.Verify(repo => repo.Get(It.Is<int>(x => x == 1)), Times.Once);
        }

        [Fact]
        public void TestGetInvalidUserById_ExpectNull()
        {
            // ARRANGE
            UserService us = new UserService(_repoMock.Object);

            // ACT
            var result = us.Get(1);

            // ASSERT
            Assert.Null(result);
            _repoMock.Verify(repo => repo.Get(It.Is<int>(x => x == 1)), Times.Once);

        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllUsers(int listCount)
        {
            // ARRANGE
            var listOfUsers = new List<User>()
            {
                new User() {UserId = 1},
                new User() {UserId = 1}
            };

            UserService us = new UserService(_repoMock.Object);

            _repoMock.Setup(x => x.GetAll()).Returns(() => listOfUsers.GetRange(0, listCount));

            // ACT
            var usersFound = us.GetAll();

            // ASSERT
            Assert.Equal(listOfUsers.GetRange(0, listCount), usersFound);
            _repoMock.Verify(repo => repo.GetAll(), Times.Once);
        }


        #endregion


    }
}
