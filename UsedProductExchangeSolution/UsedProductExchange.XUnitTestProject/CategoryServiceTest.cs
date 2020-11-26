using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using Moq;
using UsedProductExchange.Core.Application.Implementation;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using Xunit;

namespace UsedProductExchange.XUnitTestProject
{
    public class CategoryServiceTest
    {
        private List<Category> _categories = null;
        private readonly Mock<IRepository<Category>> _repoMock;

        public CategoryServiceTest()
        {
            _repoMock = new Mock<IRepository<Category>>();
            _repoMock.Setup(repo => repo.GetAll()).Returns(() => _categories);
        }
        
        #region ServiceTest
        [Fact]
        public void CreateCategoryServiceWithRepository()
        {
            // ARRANGE
            var repo = _repoMock.Object;

            // ACT
            var service = new CategoryService(repo);

            // ASSERT
            Assert.NotNull(service);
        }
        
        [Fact]
        public void CreateUserService_InvalidRepository()
        {
            // ARRANGE
            CategoryService service = null;

            // ACT
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                service = new CategoryService(null);
            });

            // ASSERT
            Assert.Equal("Repository is missing", ex.Message);
            Assert.Null(service);
        }
        #endregion
        
        #region AddCategoryTest
        [Theory]
        [InlineData(1, "Technology")]
        public void TestIfNewCategoryIsCreated_ValidInput(int id, string name)
        {
            // ARRANGE
            Category category = new Category()
            {
                CategoryId = id,
                Name = name,
            };
            CategoryService us = new CategoryService(_repoMock.Object);

            var categoryList = new List<Category>();

            // ACT
            var newCategory = us.Add(category);
            categoryList.Add(category);
            

            // ASSERT
            _repoMock.Setup(u => u.Add(category)).Returns(newCategory);
            _repoMock.Verify(repo => repo.Add(category), Times.Once);
            categoryList.Should().Contain(category);
        }
        
        [Theory]
        [InlineData(1, "", "name")] // Name is empty
        [InlineData(2, null, "name")] // Name is null
        public void CreateNewCategoryWithInvalidInput_ExpectArgumentException(int id, string name, string errorField)
        {
            // ARRANGE
            Category category = new Category()
            {
                CategoryId = id,
                Name = name,
            };

            CategoryService us = new CategoryService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<ArgumentException>(() => us.Add(category));

            // ASSERT
            Assert.Equal($"Invalid category property: {errorField}", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<Category>(c => c == category)), Times.Never);
        }
        
        [Fact]
        public void AddCategoryThatExists_ExpectInvalidArgumentException()
        {
            // ARRANGE
            Category category = new Category()
            {
                CategoryId = 1,
                Name = "Technology",
            };

            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == category.CategoryId))).Returns(() => category);

            CategoryService us = new CategoryService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => us.Add(category));

            // ASSERT
            Assert.Equal("Category already exists", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<Category>(c => c == category)), Times.Never);
        }
        #endregion
        
        #region DeleteCategoryTest
        
        #endregion
    }
}
