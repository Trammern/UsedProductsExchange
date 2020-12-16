using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UsedProductExchange.Core.Application.Implementation;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using Xunit;
using FluentAssertions;

namespace UsedProductExchange.XUnitTestProject
{
    public class ProductServiceTest
    {
        private List<Product> _products = null;
        private readonly Mock<IRepository<Product>> _repoMock;

        public ProductServiceTest()
        {
            _repoMock = new Mock<IRepository<Product>>();
            _repoMock.Setup(repo => repo.GetAll()).Returns(() => _products);
        }

        [Fact]
        public void CreateProductServiceWithRepository()
        {
            // ARRANGE
            var repo = _repoMock.Object;

            // ACT
            var service = new ProductService(repo);

            // ASSERT
            Assert.NotNull(service);
        }

        [Fact]
        public void CreateProductService_InvalidRepository()
        {
            // ARRANGE
            ProductService service = null;

            // ACT
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                service = new ProductService(null);
            });

            // ASSERT
            Assert.Equal("Repository is missing", ex.Message);
            Assert.Null(service);
        }


        [Theory]
        [ClassData(typeof(ProductClassData))]
        //[InlineData(1, 1, "Blikspand", "Fyldt med Huller", "DestinationError.png", 1000.00, null, 1)]
        public void TestIfNewProductCreatedIsCalled(int id, int uid, string name, string desc, string pic, double price, DateTime experation, Category category)
        {
            // ARRANGE
            experation = DateTime.Now.AddDays(1);
            _products = new List<Product>();
            Product product = new Product()
            {
                Category = category,
                ProductId = id,
                Name = name,
                Description = desc,
                PictureUrl = pic,
                CurrentPrice = price,
                Expiration = experation,
                UserId = uid

            };
            ProductService ps = new ProductService(_repoMock.Object);

            //ACT
            var newProduct = ps.Add(product);

            //ASSERT
            _repoMock.Verify(repo => repo.Add(product), Times.Once);

        }
        [Theory]
        [ClassData(typeof(ProductClassData))]
        public void TestIfCreatedProductIsTheSameAsTheInsertedProduct(int id, int uid, string name, string desc, string pic, double price, DateTime experation, Category category)
        {
            //ARRANGE

            experation = DateTime.Now.AddDays(1);

            var insertedProduct = new Product()
            {
                Category = category,
                ProductId = id,
                Name = name,
                Description = desc,
                PictureUrl = pic,
                CurrentPrice = price,
                Expiration = experation,
                UserId = uid
            };
            ProductService ps = new ProductService(_repoMock.Object);
            _repoMock.Setup(repo => repo.Add(insertedProduct)).Returns(() => insertedProduct);
            //ACT
            var createdProduct = ps.Add(insertedProduct);

            //ASSERT
            Assert.Equal(insertedProduct, createdProduct);

        }

        [Fact]
        public void RemoveExistingProduct()
        {
            // ARRANGE
            var product = new Product()
            {
                Category = new Category(){Name = "Test Category"},
                ProductId = 1,
                Name = "Blikspand",
                Description = "Lavet af ler",
                PictureUrl = "URLISGONE.PNG",
                CurrentPrice = 1000.00,
                Expiration = DateTime.Now,
                UserId = 1
            };

            ProductService ps = new ProductService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(p => p == product.ProductId))).Returns(() => product);

            // ACT
            var deletedProduct = ps.Delete(product.ProductId);

            // ASSERT
            _repoMock.Verify(repo => repo.Remove(It.Is<int>(u => u == product.ProductId)), Times.Once);
            deletedProduct.Should().BeNull();
        }
      
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void TestIdGetAllProductsReturnCorrectProduct(int productlist)
        {
            //ARRANGE
            var products = new List<Product>()
            {
                new Product() {ProductId = 1,
                    Name = "Hest"},
                new Product() {ProductId = 2,
                    Name = "Hest"}
            };

            _repoMock.Setup(x => x.GetAll()).Returns(() => products.GetRange(0, productlist));
            ProductService ps = new ProductService(_repoMock.Object);

            //ACT
            var result = ps.GetAll();

            //ASSERT
            Assert.Equal(products.GetRange(0, productlist), result);
            _repoMock.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(77, 77)]

        public void TestIfGetProductByIdReturnCorrectProduct(int searchId, int expected)
        {
            //ARRANGE
            var products = new List<Product>()
            {
                new Product()
                {
                    ProductId = 1,
                    Name = "Hest"
                    
                },
                new Product()
                {
                    ProductId = 2,
                    Name = "Blikspand"
                },
                new Product()
                {
                    ProductId = 77,
                    Name = "Staldør"

                }
            };
            _repoMock.Setup(repo => repo.Get(searchId)).Returns(products.Where(p => p.ProductId == searchId).FirstOrDefault);
            ProductService ps = new ProductService(_repoMock.Object);

            //ACT
            var foundProduct = ps.Get(searchId);

            //ASSERT
            Assert.Equal(searchId, expected);
            
        }

        [Fact]
        public void TestIfUpdateRepositoryMethodIsCalled()
        {

            // ARRANGE
            ProductService ps = new ProductService(_repoMock.Object);
            Product product = new Product(){
                Name = "Hest"};

            //ACT
            var updatedProduct = ps.Update(product);

            //ASSERT
            _repoMock.Verify(repo => repo.Edit(product), Times.Once);
        }
        [Fact]
        public void TestIfProductIsNotValidThrowsInvalidOperationExeption()
        {
            //Arrange
            Product product = new Product()
            {
                ProductId = 50
            };
            _repoMock.Setup(repo => repo.Get(It.Is<int>(z => z == product.ProductId))).Returns(() => null);
            ProductService ps = new ProductService(_repoMock.Object);

            //ACT +ASSERT
            var ex = Assert.Throws<InvalidOperationException>(()=> ps.Update(product));

            Assert.Equal("Product must have a name", ex.Message);
            _repoMock.Verify(repo => repo.Edit(It.Is<Product>(p => p == product)), Times.Never);
        }

        [Fact]
        public void AddProductWhoExists_ExpectInvalidArgumentException()
        {
            // ARRANGE
            Product product = new Product()
            {
                Category = new Category() { Name = "Test Category "},
                ProductId = 1,
                Name = "Blikspand",
                Description = "Lavet af ler",
                PictureUrl = "URLISGONE.PNG",
                CurrentPrice = 1000.00,
                Expiration = DateTime.Now.AddDays(1),
                UserId = 1
            };

            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == product.ProductId))).Returns(() => product);

            ProductService us = new ProductService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => us.Add(product));

            // ASSERT
            Assert.Equal("Product already exists", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<Product>(p => p == product)), Times.Never);
        }

        [Theory]
        [InlineData("2019-01-01")]
        [InlineData("2020-01-01")]
        [InlineData("2000-01-01")]
        [InlineData("872-01-01")]
        public void TestIfDateOnBidIsOlderThanCurrentDateWhenCreated(string time)
        {
            //ARRANGE

            _products = new List<Product>();

            ProductService ps = new ProductService(_repoMock.Object);

            Product p = new Product()
            {
                ProductId = 1,
                Name = "something",
                CurrentPrice = 200,
                IsSold = false,
                Expiration = DateTime.Parse(time),
                Description = "something",
                UserId = 1,
                CategoryId = 1,
                Category = null,
                Bids = null,
                PictureUrl = "no"
            };

            //ACT

            var ex = Assert.Throws<ArgumentException>(() => ps.Add(p));

            //ASSERT

            Assert.Equal("Auction end date must be after today", ex.Message);
        }

        [Theory]
        [ClassData(typeof(ProductClassInvalidData))]
        public void CreateNewProductWithInvalidInput_ExpectArgumentException(int id, int uid, string name, string desc, string pic, double price, DateTime expiration, Category category, string errorField)
        {
            // ARRANGE
            Product product = new Product()
            {
                Category = category,
                ProductId = id,
                Name = name,
                Description = desc,
                PictureUrl = pic,
                CurrentPrice = price,
                Expiration = expiration,
                UserId = uid
            };

            ProductService ps = new ProductService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<ArgumentException>(() => ps.Add(product));

            // ASSERT
            Assert.Equal($"Invalid product property: {errorField}", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<Product>(u => u == product)), Times.Never);
        }
    }
    
    public class ProductClassInvalidData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 1, "", "desc", "pic", 1000.00, null, new Category() { Name = "Test Category "}, "name" }; // Name is empty
            yield return new object[] { 1, 1, null, "desc", "pic", 1000.00, null, new Category() { Name = "Test Category "}, "name" }; // Name is null
            yield return new object[] { 1, 1, "name", "", "pic", 1000.00, null, new Category() { Name = "Test Category "}, "description" }; // desc is empty
            yield return new object[] { 1, 1, "name", null, "pic", 1000.00, null, new Category() { Name = "Test Category "}, "description" }; // desc is null
            yield return new object[] { 1, 1, "name", "desc", "", 1000.00, null, new Category() { Name = "Test Category "}, "Picture" }; // Pic is empty
            yield return new object[] { 1, 1, "name", "desc", null, 1000.00, null, new Category() { Name = "Test Category "}, "Picture" }; // Pic is empty
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    
    public class ProductClassData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 1, 1, "Blikspand", "Fyldt med Huller", "DestinationError.png", 1000.00, null, new Category() { Name = "Test Category "} };
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}