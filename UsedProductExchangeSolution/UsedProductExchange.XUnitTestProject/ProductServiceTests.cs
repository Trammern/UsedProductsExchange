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

        public void TestIfNewProductIsCreated(int id, int uid, string name, string desc, string pic, double price, DateTime experation, int category)
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

    }
}


