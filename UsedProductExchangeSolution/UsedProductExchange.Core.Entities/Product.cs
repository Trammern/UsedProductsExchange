using System;
using System.Collections.Generic;
using System.Text;

namespace UsedProductExchange.Core.Entities
{
    public class Product
    {
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PictureURL { get; set; }
        public double CurrentPrice { get; set; }
        public DateTime Expiration { get; set; }
        public int CategoryId { get; set; }
    }
}
