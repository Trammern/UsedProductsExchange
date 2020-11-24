using Moq;
using System;
using System.Collections.Generic;
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

        public void TestIdGetAllProductsIsCalled(int productlist)
        {
            //ARRANGE
            var products = new List<Product>()
            {
                new Product() {ProductId = 1},
                new Product() {ProductId = 2}
            };

            repoMock.Setup(x => x.GetAllProducts()).Returns(() => products.GetRange(0, productlist));
            ProductService ps = new ProductService(repoMock.Object);

            //ACT
            var result = ps.GetAllProduct();

            //ASSERT
            Assert.Equal(products.GetRange(0, productlist), result);
            repoMock.Verify(repo => repo.GetAllProducts(), Times.Once);
        }


    }
}


