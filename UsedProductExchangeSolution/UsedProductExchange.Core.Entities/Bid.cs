using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsedProductExchange.Core.Entities
{
    public class Bid
    {
        [Key]
        public int BidId { get; set; }
        [Column(Order = 0)]
        public int UserId { get; set; }
        [Column(Order = 1)]
        public int ProductId { get; set; }

        public virtual User User { get; set; }
        public virtual Product Product { get; set; }
        
        public double Price { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}