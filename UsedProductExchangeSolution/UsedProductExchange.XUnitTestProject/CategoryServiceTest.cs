using System;
using System.Collections.Generic;
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
        public void CreateCategoryService_InvalidRepository()
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
                Products = new List<Product>()
                {
                    new Product()
                    {
                        ProductId = 1,
                        CurrentPrice = 1000,
                        Description = "Hello",
                        Expiration = DateTime.Now,
                        Name = "Test Product",
                        PictureUrl = "pic.jpg",
                        UserId = 1,
                    },
                    new Product() {
                        ProductId = 2,
                        CurrentPrice = 100,
                        Description = "Hello 2",
                        Expiration = DateTime.Now,
                        Name = "Test 2 Product",
                        PictureUrl = "pic2.jpg",
                        UserId = 1,
                    }
                }
            };
            
            CategoryService cs = new CategoryService(_repoMock.Object);

            var categoryList = new List<Category>();

            // ACT
            var newCategory = cs.Add(category);
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

            CategoryService cs = new CategoryService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<ArgumentException>(() => cs.Add(category));

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

            CategoryService cs = new CategoryService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => cs.Add(category));

            // ASSERT
            Assert.Equal("Category already exists", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<Category>(c => c == category)), Times.Never);
        }
        #endregion
        
        #region DeleteCategoryTest
        [Fact]
        public void RemoveExistingCategory()
        {
            // ARRANGE
            Category category = new Category()
            {
                CategoryId = 1,
                Name = "Technology",
            };

            CategoryService cs = new CategoryService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(c => c == category.CategoryId))).Returns(() => category);

            // ACT
            var deletedCategory = cs.Delete(category.CategoryId);

            // ASSERT
            _repoMock.Verify(repo => repo.Remove(It.Is<int>(c => c == category.CategoryId)), Times.Once);
            deletedCategory.Should().BeNull();
        }
        
        [Fact]
        public void DeleteCategoryNotFound_ExpectInvalidOperationException()
        {
            // ARRANGE
            Category category = new Category()
            {
                CategoryId = 1,
                Name = "Technology",
            };

            CategoryService cs = new CategoryService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => cs.Delete(category.CategoryId));

            // ASSERT
            Assert.Equal("Category not found", ex.Message);
            _repoMock.Verify(repo => repo.Remove(It.Is<int>(c => c == category.CategoryId)), Times.Never);
        }
        #endregion
        
        #region UpdateCategoryTest
        [Theory]
        [InlineData(1, "some name")]
        [InlineData(1, "another name")]
        public void UpdateValidCategory(int id, string name)
        {
            // ARRANGE
            Category category = new Category()
            {
                CategoryId = id,
                Name = name,
            };

            CategoryService cs = new CategoryService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(z => z == category.CategoryId))).Returns(() => category);

            // ACT
            var updatedCategory = cs.Update(category);

            // ASSERT
            _repoMock.Verify(repo => repo.Edit(It.Is<Category>(c => c == category)), Times.Once);
        }

        [Fact]
        public void UpdatingCategoryNotFound_ExpectInvalidOperationException()
        {
            // ARRANGE
            Category category = new Category()
            {
                CategoryId = 1,
                Name = "Technology",
            };

            CategoryService cs = new CategoryService(_repoMock.Object);

            // check if not existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == category.CategoryId))).Returns(() => null);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => cs.Update(category));

            // ASSERT
            Assert.Equal("Category to update not found", ex.Message);
            _repoMock.Verify(repo => repo.Edit(It.Is<Category>(c => c == category)), Times.Never);

        }

        [Fact]
        public void UpdatedCategoryShouldReplaceOldCategory()
        {
            // ARRANGE
            Category category = new Category()
            {
                CategoryId = 1,
                Name = "Technology"
            };
            CategoryService cs = new CategoryService(_repoMock.Object);

            // check if category or id is not null
            _repoMock.Setup(c => c.Get(It.Is<int>(id => id == category.CategoryId))).Returns(() => category);
            _repoMock.Setup(c => c.Edit(category)).Returns(category);

            // ACT
            var updatedCategory = cs.Update(category);

            // ASSERT (Fluent)
            updatedCategory.Should().Be(category);
            _repoMock.Verify(repo => repo.Edit(It.Is<Category>(c => c == category)), Times.Once);
        }
        #endregion

        #region ReadCategoryTest

        [Fact]
        public void TestGetExistingCategoryById()
        {
            // ARRANGE
            Category category = new Category()
            {
                CategoryId = 1,
                Name = "name",
            };
            
            CategoryService cs = new CategoryService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == category.CategoryId))).Returns(() => category);

            // ACT
            var categoryFound = cs.Get(1);

            // ASSERT
            Assert.Equal(category, categoryFound);
            _repoMock.Verify(repo => repo.Get(It.Is<int>(x => x == 1)), Times.Once);
        }
        
        [Fact]
        public void TestGetInvalidCategoryById_ExpectNull()
        {
            // ARRANGE
            CategoryService cs = new CategoryService(_repoMock.Object);

            // ACT
            var result = cs.Get(1);

            // ASSERT
            Assert.Null(result);
            _repoMock.Verify(repo => repo.Get(It.Is<int>(x => x == 1)), Times.Once);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllCategories(int listCount)
        {
            // ARRANGE
            var listOfCategories = new List<Category>()
            {
                new Category() {CategoryId = 1},
                new Category() {CategoryId = 1}
            };

            CategoryService cs = new CategoryService(_repoMock.Object);

            _repoMock.Setup(x => x.GetAll()).Returns(() => listOfCategories.GetRange(0, listCount));

            // ACT
            var categoriesFound = cs.GetAll();

            // ASSERT
            Assert.Equal(listOfCategories.GetRange(0, listCount), categoriesFound);
            _repoMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        #endregion
    }
}
