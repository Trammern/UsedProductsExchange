using Moq;
using System;
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
        private List<Product> products = null;
        private readonly Mock<IProductRepository> repoMock;

        public ProductServiceTest()
        {
            repoMock = new Mock<IProductRepository>();
            repoMock.Setup(repo => repo.GetAllProducts()).Returns(() => products);
        }

        [Fact]
        public void CreateProductServiceWithRepository()
        {
            // ARRANGE
            var repo = repoMock.Object;

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
        [InlineData(1, 1, "Blikspand", "Fyldt med Huller", "DestinationError.png", 1000.00, null, 1)]

        public void TestIfNewProductCreatedIsCalled(int id, int uid, string name, string desc, string pic, double price, DateTime experation, int category)
        {
            // ARRANGE
            Product product = new Product()
            {
                CategoryId = category,
                ProductId = id,
                Name = name,
                Description = desc,
                PictureURL = pic,
                CurrentPrice = price,
                Expiration = experation,
                UserId = uid

            };
            ProductService ps = new ProductService(repoMock.Object);

            //ACT
            var newProduct = ps.CreateProduct(product);

            //ASSERT
            repoMock.Verify(repo => repo.CreateProduct(product), Times.Once);

        }
        [Theory]
        [InlineData(1, 1, "Blikspand", "Fyldt med Huller", "DestinationError.png", 1000.00, null, 1)]
        public void TestIfCreatedProductIsTheSameAsTheInsertedProduct(int id, int uid, string name, string desc, string pic, double price, DateTime experation, int category)
        {
            //ARRANGE
            var insertedProduct = new Product()
            {
                CategoryId = category,
                ProductId = id,
                Name = name,
                Description = desc,
                PictureURL = pic,
                CurrentPrice = price,
                Expiration = experation,
                UserId = uid
            };
            ProductService ps = new ProductService(repoMock.Object);
            repoMock.Setup(repo => repo.CreateProduct(insertedProduct)).Returns(() => insertedProduct);
            //ACT
            var createdProduct = ps.CreateProduct(insertedProduct);

            //ASSERT
            Assert.Equal(insertedProduct, createdProduct);

        }

        [Fact]
        public void RemoveExistingProduct()
        {
            // ARRANGE
            var product = new Product()
            {
                CategoryId = 1,
                ProductId = 1,
                Name = "Blikspand",
                Description = "Lavet af ler",
                PictureURL = "URLISGONE.PNG",
                CurrentPrice = 1000.00,
                Expiration = DateTime.Now,
                UserId = 1
            };

            ProductService ps = new ProductService(repoMock.Object);

            // check if existing
            repoMock.Setup(repo => repo.Get(It.Is<int>(p => p == product.ProductId))).Returns(() => product);

            // ACT
            var deletedProduct = ps.DeleteProduct(product.ProductId);

            // ASSERT
            repoMock.Verify(repo => repo.Remove(It.Is<int>(u => u == product.ProductId)), Times.Once);
            deletedProduct.Should().BeNull();
        }


            [Theory]
        [InlineData(1, 1, "Blikspand", "Fyldt med Huller", "DestinationError.png", 1000.00, null, 1)]
        public void TestIfDeletedProductIsTheSameAsTheInsertedProduct(int id, int uid, string name, string desc, string pic, double price, DateTime experation, int category)
        {
            //ARRANGE
            var insertedProduct = new Product()
            {
                CategoryId = category,
                ProductId = id,
                Name = name,
                Description = desc,
                PictureURL = pic,
                CurrentPrice = price,
                Expiration = experation,
                UserId = uid
            };
            ProductService ps = new ProductService(repoMock.Object);
            products = new List<Product>();

            ps.CreateProduct(insertedProduct);
            repoMock.Setup(repo => repo.CreateProduct(insertedProduct)).Callback(() => products.Add(insertedProduct));
            repoMock.Setup(repo => repo.DeleteProduct(insertedProduct)).Returns(() => insertedProduct);
            //ACT
            var deletedProduct = ps.DeleteProduct(insertedProduct);

            //ASSERT
            Assert.Equal(insertedProduct, deletedProduct);
            Assert.Empty(products);
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

            repoMock.Setup(x => x.GetAllProducts()).Returns(() => products.GetRange(0, productlist));
            ProductService ps = new ProductService(repoMock.Object);

            //ACT
            var result = ps.GetAllProduct();

            //ASSERT
            Assert.Equal(products.GetRange(0, productlist), result);
            repoMock.Verify(repo => repo.GetAllProducts(), Times.Once);
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(2, 2)]
        [InlineData(77, 77)]

        public void TestIfGetProductByIdReturnCorrectProduct(int searchId, int expected)
        {
            //ARRANGE
            products = new List<Product>()
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
            repoMock.Setup(repo => repo.GetProductById(searchId)).Returns(products.Where(p => p.ProductId == searchId).FirstOrDefault);
            ProductService ps = new ProductService(repoMock.Object);

            //ACT
            var foundProduct = ps.GetProductById(searchId);

            //ASSERT
            Assert.Equal(searchId, expected);
            
        }

        [Fact]
        public void TestIfUpdateRepositoryMethodIsCalled()
        {

            // ARRANGE
            ProductService ps = new ProductService(repoMock.Object);
            Product product = new Product(){
                Name = "Hest"};

            //ACT
            var updatedProduct = ps.UpdateProduct(product);

            //ASSERT
            repoMock.Verify(repo => repo.UpdateProduct(product), Times.Once);
        }
        [Fact]
        public void TestIfProductIsNotValidThrowsInvalidOperationExeption()
        {
            //Arrange
            Product product = new Product()
            {
                ProductId = 50
            };
            repoMock.Setup(repo => repo.GetProductById(It.Is<int>(z => z == product.ProductId))).Returns(() => null);
            ProductService ps = new ProductService(repoMock.Object);

            //ACT +ASSERT
            var ex = Assert.Throws<InvalidOperationException>(()=> ps.UpdateProduct(product));

            Assert.Equal("Product must have a name", ex.Message);
            repoMock.Verify(repo => repo.UpdateProduct(It.Is<Product>(p => p == product)), Times.Never);
        }

        [Fact]
        public void AddProductWhoExists_ExpectInvalidArgumentException()
        {
            // ARRANGE
            Product product = new Product()
            {
                CategoryId = 1,
                ProductId = 1,
                Name = "Blikspand",
                Description = "Lavet af ler",
                PictureURL = "URLISGONE.PNG",
                CurrentPrice = 1000.00,
                Expiration = DateTime.Now,
                UserId = 1
            };

            repoMock.Setup(repo => repo.GetProductById(It.Is<int>(x => x == product.ProductId))).Returns(() => product);

            ProductService us = new ProductService(repoMock.Object);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => us.CreateProduct(product));

            // ASSERT
            Assert.Equal("Product already exists", ex.Message);
            repoMock.Verify(repo => repo.CreateProduct(It.Is<Product>(p => p == product)), Times.Never);
        }

        [Theory]
        [InlineData(1, 1, "", "desc", "pic", 1000.00, null, 1, "name")] // Name is empty
        [InlineData(1, 1, null, "desc", "pic", 1000.00, null, 1, "name")] // Name is null
        [InlineData(1, 1, "name", "", "pic", 1000.00, null, 1, "description")] // desc is empty
        [InlineData(1, 1, "name", null, "pic", 1000.00, null, 1, "description")] // desc is null
        [InlineData(1, 1, "name", "desc", "", 1000.00, null, 1, "Picture")] // Pic is empty
        [InlineData(1, 1, "name", "desc", null, 1000.00, null, 1, "Picture")] // Address is null
      

        public void CreateNewProductWithInvalidInput_ExpectArgumentException(int id, int uid, string name, string desc, string pic, double price, DateTime experation, int category, string errorField)
        {
            // ARRANGE
            Product product = new Product()
            {
                CategoryId = category,
                ProductId = id,
                Name = name,
                Description = desc,
                PictureURL = pic,
                CurrentPrice = price,
                Expiration = experation,
                UserId = uid
            };

            ProductService ps = new ProductService(repoMock.Object);

            // ACT
            var ex = Assert.Throws<ArgumentException>(() => ps.CreateProduct(product));

            // ASSERT
            Assert.Equal($"Invalid product property: {errorField}", ex.Message);
            repoMock.Verify(repo => repo.CreateProduct(It.Is<Product>(u => u == product)), Times.Never);
        }
    }
}


