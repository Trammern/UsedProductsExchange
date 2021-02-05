using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using UsedProductExchange.Core.Domain;
using UsedProductExchange.Core.Entities;
using UsedProductExchange.Core.Filter;
using UsedProductExchange.Infrastructure.Context;
using System.Linq;

namespace UsedProductExchange.Infrastructure.Repositories
{
    public class BidRepository: IRepository<Bid>
    {
        private readonly UsedProductExchangeContext _ctx;
        private readonly Mail mail;

        public BidRepository(UsedProductExchangeContext ctx)
        {
            _ctx = ctx;
            mail = new Mail();
        }
        
        public FilteredList<Bid> GetAll(Filter filter)
        {
            var filteredList = new FilteredList<Bid>
            {
                TotalCount = _ctx.Bids.Count(),
                FilterUsed = filter,
                List = _ctx.Bids.Select(b => new Bid()
                    {
                        BidId = b.BidId, 
                        UserId = b.UserId,
                        ProductId = b.ProductId,
                        Price = b.Price,
                        CreatedAt = b.CreatedAt
                    })
                    .ToList()
            };
            return filteredList;
        }

        public IEnumerable<Bid> GetAll()
        {
            return _ctx.Bids.AsNoTracking();
        }

        public Bid Get(int id)
        {
            return _ctx.Bids
                .Include(p => p.Product)
                .Include(u => u.User)
                .AsNoTracking()
                .FirstOrDefault(x => x.BidId == id);
        }

        public Bid Add(Bid entity)
        {
            // Add bid
            var bid = _ctx.Bids.Add(entity);
            _ctx.SaveChanges();
            
            // Send email to seller
            // mail.SendSimpleMessage("andreasbendorff@gmail.com", 
            //      "Someone got a bid", 
            //      bid.Entity.User.Name + " just bid " + bid.Entity.Price + " on your item.");
            
            // Return the bid
            return bid.Entity;
        }

        public Bid Edit(Bid entity)
        {
            _ctx.Entry(entity).State = EntityState.Modified;
            _ctx.SaveChanges();
            return entity;
        }

        public Bid Remove(int id)
        {
            var bid = _ctx.Bids.FirstOrDefault(x => x.BidId == id);
            if(bid == null) throw new ArgumentException("Bid does not exist");
            var deletedBid = _ctx.Bids.Remove(bid);
            _ctx.SaveChanges();
            return deletedBid.Entity;
        }
    }
}