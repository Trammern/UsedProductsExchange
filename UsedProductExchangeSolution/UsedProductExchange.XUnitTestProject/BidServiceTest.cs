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
    public class BidServiceTest
    {
        private List<Bid> _bids = null;
        private readonly Mock<IRepository<Bid>> _repoMock;

        public BidServiceTest()
        {
            _repoMock = new Mock<IRepository<Bid>>();
            _repoMock.Setup(repo => repo.GetAll()).Returns(() => _bids);
        }

        #region ServiceTest

        [Fact]
        public void CreateBidServiceWithRepository()
        {
            // ARRANGE
            var repo = _repoMock.Object;

            // ACT
            var service = new BidService(repo);

            // ASSERT
            Assert.NotNull(service);
        }

        [Fact]
        public void CreateBidService_InvalidRepository()
        {
            // ARRANGE
            BidService service = null;

            // ACT
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                service = new BidService(null);
            });

            // ASSERT
            Assert.Equal("Repository is missing", ex.Message);
            Assert.Null(service);
        }

        #endregion

        #region AddBidTest

        [Theory]
        [InlineData(1, 1, 100.00)]
        public void TestIfNewBidIsCreated_ValidInput(int userid, int productId, double price)
        {
            // ARRANGE
            var bid = new Bid()
            {
                UserId = userid,
                ProductId = productId,
                Price = price,
                CreatedAt = DateTime.Now.AddDays(1),
                Product = new Product()
                {
                    Name = "test"
                }
                
            };

            var bs = new BidService(_repoMock.Object);

            var bidList = new List<Bid>();

            // ACT
            var newBid = bs.Add(bid);
            bidList.Add(bid);


            // ASSERT
            _repoMock.Setup(b => b.Add(bid)).Returns(newBid);
            _repoMock.Verify(repo => repo.Add(bid), Times.Once);
            bidList.Should().Contain(bid);
        }

        [Theory]
        [InlineData(null, 1, 100.00, "userId")] // UserId is null
        [InlineData(2, null, 200.00, "productId")] // ProductId is null
        [InlineData(0, 1, 200.00, "userId")] // UserId is less than one
        [InlineData(2, 0, 200.00, "productId")] // ProductId is less than one
        [InlineData(2, 1, -1.00, "price")] // price is less than 0
        public void CreateNewBidWithInvalidInput_ExpectArgumentException(int userid, int productId, double price, string errorField)
        {
            // ARRANGE
            var bid = new Bid()
            {
                UserId = userid,
                ProductId = productId,
                Price = price,
                CreatedAt = DateTime.Now
            };

            var bs = new BidService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<ArgumentException>(() => bs.Add(bid));

            // ASSERT
            Assert.Equal($"Invalid bid property: {errorField}", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<Bid>(b => b == bid)), Times.Never);
        }

        [Fact]
        public void AddBidThatExists_ExpectInvalidArgumentException()
        {
            // ARRANGE
            var bid = new Bid()
            {
                UserId = 1,
                ProductId = 1,
                Price = 2000,
                CreatedAt = DateTime.Now
            };

            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == bid.BidId))).Returns(() => bid);

            var bs = new BidService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => bs.Add(bid));

            // ASSERT
            Assert.Equal("Bid already exists", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<Bid>(b => b == bid)), Times.Never);
        }

        [Theory]
        [InlineData(2, 1, 200)] //Lower than highest price
        [InlineData(2, 1, 2000)] //Equal to higher price
        public void AddBidWithPriceLowerThanCurrentHighest_ExpectInvalidArgumentException(int userId, int productId, double price)
        {


            //ARRANGE
            var bid1 = new Bid()
            {
                BidId = 1,
                UserId = 1,
                ProductId = 1,
                Price = 2000,
                CreatedAt = DateTime.Now
            };

            var bid2 = new Bid()
            {
                BidId = 2,
                UserId = userId,
                ProductId = productId,
                Price = price,
                CreatedAt = DateTime.Now
            };

            _bids = new List<Bid>
            {
                bid1
            };

            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == bid1.BidId))).Returns(() => bid1);

            var bs = new BidService(_repoMock.Object);

            //ACT

            var ex = Assert.Throws<InvalidOperationException>(() => bs.Add(bid2));

            //ASSERT
            Assert.Equal("Bid is lower, or equal to, the current highest bid", ex.Message);
            _repoMock.Verify(repo => repo.Add(It.Is<Bid>(b => b == bid2)), Times.Never);
        }

        

        #endregion

        #region DeleteBidTest

        [Fact]
        public void RemoveExistingBid()
        {
            // ARRANGE
            var bid = new Bid()
            {
                UserId = 1,
                ProductId = 1,
                Price = 100,
                CreatedAt = DateTime.Now
            };

            var bs = new BidService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(b => b == bid.BidId))).Returns(() => bid);

            // ACT
            var deletedBid = bs.Delete(bid.BidId);

            // ASSERT
            _repoMock.Verify(repo => repo.Remove(It.Is<int>(b => b == bid.BidId)), Times.Once);
            deletedBid.Should().BeNull();
        }
        
        [Fact]
        public void DeleteBidNotFound_ExpectInvalidOperationException()
        {
            // ARRANGE
            var bid = new Bid()
            {
                UserId = 1,
                ProductId = 1,
                Price = 100,
                CreatedAt = DateTime.Now
            };

            var bs = new BidService(_repoMock.Object);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => bs.Delete(bid.BidId));

            // ASSERT
            Assert.Equal("Bid not found", ex.Message);
            _repoMock.Verify(repo => repo.Remove(It.Is<int>(b => b == bid.BidId)), Times.Never);
        }

        #endregion

        #region UpdateBidTest

        [Theory]
        [InlineData(1, 1, 100)]
        [InlineData(1, 1, 200)]
        public void UpdateValidBid(int userid, int productId, int price)
        {
            // ARRANGE
            var bid = new Bid()
            {
                UserId = userid,
                ProductId = productId,
                Price = price,
                CreatedAt = DateTime.Now.AddDays(1),
                Product = new Product()
                {
                    Name = "test"
                }
            };

            var bs = new BidService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(z => z == bid.BidId))).Returns(() => bid);

            // ACT
            var updatedBid = bs.Update(bid);

            // ASSERT
            _repoMock.Verify(repo => repo.Edit(It.Is<Bid>(b => b == bid)), Times.Once);
        }

        [Fact]
        public void UpdatingBidNotFound_ExpectInvalidOperationException()
        {
            // ARRANGE
            var bid = new Bid()
            {
                UserId = 1,
                ProductId = 1,
                Price = 1020,
                CreatedAt = DateTime.Now.AddDays(1),
                Product = new Product()
                {
                    Name = "test"
                }
            };

            var bs = new BidService(_repoMock.Object);

            // check if not existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == bid.BidId))).Returns(() => null);

            // ACT
            var ex = Assert.Throws<InvalidOperationException>(() => bs.Update(bid));

            // ASSERT
            Assert.Equal("Bid to update not found", ex.Message);
            _repoMock.Verify(repo => repo.Edit(It.Is<Bid>(b => b == bid)), Times.Never);

        }

        [Fact]
        public void UpdatedBidShouldReplaceOldBid()
        {
            // ARRANGE
            var bid = new Bid()
            {
                UserId = 1,
                ProductId = 1,
                Price = 1020,
                CreatedAt = DateTime.Now.AddDays(1),
                Product = new Product()
                {
                    Name = "test"
                }
            };
            var bs = new BidService(_repoMock.Object);

            // check if bid or id is not null
            _repoMock.Setup(b => b.Get(It.Is<int>(id => id == bid.BidId))).Returns(() => bid);
            _repoMock.Setup(b => b.Edit(bid)).Returns(bid);

            // ACT
            var updatedBid = bs.Update(bid);

            // ASSERT (Fluent)
            updatedBid.Should().Be(bid);
            _repoMock.Verify(repo => repo.Edit(It.Is<Bid>(b => b == bid)), Times.Once);
        }

        #endregion

        #region ReadBidTest

        [Fact]
        public void TestGetExistingBidById()
        {
            // ARRANGE
            var bid = new Bid()
            {
                BidId = 1,
                UserId = 1,
                ProductId = 1,
                Price = 2000,
                CreatedAt = DateTime.Now
            };
            
            var bs = new BidService(_repoMock.Object);

            // check if existing
            _repoMock.Setup(repo => repo.Get(It.Is<int>(x => x == bid.BidId))).Returns(() => bid);

            // ACT
            var bidFound = bs.Get(1);

            // ASSERT
            Assert.Equal(bid, bidFound);
            _repoMock.Verify(repo => repo.Get(It.Is<int>(x => x == 1)), Times.Once);
        }
        
        [Fact]
        public void TestGetInvalidBidById_ExpectNull()
        {
            // ARRANGE
            var bs = new BidService(_repoMock.Object);

            // ACT
            var result = bs.Get(1);

            // ASSERT
            Assert.Null(result);
            _repoMock.Verify(repo => repo.Get(It.Is<int>(x => x == 1)), Times.Once);
        }
        
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void GetAllBids(int listCount)
        {
            // ARRANGE
            var listOfBids = new List<Bid>()
            {
                new Bid() {BidId = 1},
                new Bid() {BidId = 1}
            };

            var bs = new BidService(_repoMock.Object);

            _repoMock.Setup(x => x.GetAll()).Returns(() => listOfBids.GetRange(0, listCount));

            // ACT
            var bidsFound = bs.GetAll();

            // ASSERT
            Assert.Equal(listOfBids.GetRange(0, listCount), bidsFound);
            _repoMock.Verify(repo => repo.GetAll(), Times.Once);
        }
        [Theory]
        [InlineData(200, 400, 6000, 6000)]
        [InlineData(200, 200, 200, 200)]
        [InlineData(400, 200, 500, 500)]
        public void GetCurrentHighestBid(double price1, double price2, double price3, double expected)
        {
            //ARRANGE

            BidService bs = new BidService(_repoMock.Object);

            _bids = new List<Bid>
            {
                new Bid()
                {
                    BidId = 1,
                    ProductId = 1,
                    Price = price1,
                },
                new Bid()
                {
                    BidId = 2,
                    ProductId = 1,
                    Price = price2
                },
                new Bid()
                {
                    BidId = 3,
                    ProductId = 1,
                    Price = price3
                }
            };

            //ACT

            Bid result = bs.GetHighestBid(new Bid { ProductId = 1 });

            //ASSERT

            Assert.Equal(result.Price, expected);
            
        }
        [Fact]
        public void GetCurrentHighestBidWhenDataBaseIsEmptyReturnsNull()
        {
            //ARRANGE

            BidService bs = new BidService(_repoMock.Object);

            //ACT

            var result = bs.GetHighestBid(new Bid() { ProductId = 1 });

            //ASSERT

            Assert.Null(result);
        }

        #endregion
    }
}