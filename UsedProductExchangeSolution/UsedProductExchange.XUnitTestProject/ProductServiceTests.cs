using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UsedProductExchange.Core.Application.Implementation;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using Xunit;

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
        public void TestIfDeleteProductIsCalled()
        {
            ProductService ps = new ProductService(repoMock.Object);

            // ARRANGE
            Product product = new Product();

            var newProduct = ps.CreateProduct(product);

            //ACT
            var deletedProduct = ps.DeleteProduct(product);

            //ASSERT
            repoMock.Verify(repo => repo.DeleteProduct(product), Times.Once);
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
    }
}


