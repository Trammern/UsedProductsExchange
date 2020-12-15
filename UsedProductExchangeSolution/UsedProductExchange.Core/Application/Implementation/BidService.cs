using System;
using System.Collections.Generic;
using System.Linq;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Filter;

namespace UsedProductExchange.Core.Application.Implementation
{
    public class BidService: IService<Bid>
    {
        private readonly IRepository<Bid> _bidRepository;
        
        public BidService(IRepository<Bid> bidRepository)
        {
            _bidRepository = bidRepository ?? throw new ArgumentException("Repository is missing");
        }
        
        private void ValidationCheck(Bid bid)
        {
            // Null or empty checks
            if (bid == null)
            {
                throw new ArgumentException("Bid is missing");
            }
            if (bid.UserId < 1)
            {
                throw new ArgumentException("Invalid bid property: userId");
            }
            if (bid.ProductId < 1)
            {
                throw new ArgumentException("Invalid bid property: productId");
            }
            if (bid.Price < 0)
            {
                throw new ArgumentException("Invalid bid property: price");
            }
            if ((int)bid.Price < (int)GetHighestBid(bid))
            {
                throw new InvalidOperationException("Bid is lower than the current, highest bid");
            }
        }
        
        public FilteredList<Bid> GetAll(Filter.Filter filter)
        {
            return _bidRepository.GetAll(filter);
        }

        public List<Bid> GetAll()
        {
            return _bidRepository.GetAll().ToList();
        }

        public Bid Get(int id)
        {
            return _bidRepository.Get(id);
        }

        public Bid Add(Bid entity)
        {
            ValidationCheck(entity);
            
            // Check if already existing
            if (_bidRepository.Get(entity.BidId) != null)
            {
                throw new InvalidOperationException("Bid already exists");
            }
            
            return _bidRepository.Add(entity);
        }

        public Bid Update(Bid entity)
        {
            ValidationCheck(entity);

            if (entity == null || _bidRepository.Get(entity.BidId) == null)
            {
                throw new InvalidOperationException("Bid to update not found");
            }
            
            return _bidRepository.Edit(entity);
        }

        public Bid Delete(int id)
        {
            if (_bidRepository.Get(id) == null)
            {
                throw new InvalidOperationException("Bid not found");
            }
            
            return _bidRepository.Remove(id);
        }

        private double GetHighestBid(Bid bid)
        { 
            
            var currentList = _bidRepository.GetAll();

            if (currentList == null || currentList.Count() == 0)
            {
                return 0;
            }

            var result = currentList.OrderBy(x => x.Price).Where(x => x.ProductId == bid.ProductId).Last();

            if (result == null)
            {
                return 0;
            }

            return result.Price;
        }
    }
}