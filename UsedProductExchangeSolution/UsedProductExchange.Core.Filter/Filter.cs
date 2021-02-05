namespace UsedProductExchange.Core.Filter
{
    public class Filter
    {
        public string OrderDirection { get; set; }
        public string OrderProperty { get; set; }
        public string SearchText { get; set; }
        public string SearchField { get; set; }
        public int ItemsPrPage { get; set; }
        public int CurrentPage { get; set; }
        public int Price { get; set; }
    }
}