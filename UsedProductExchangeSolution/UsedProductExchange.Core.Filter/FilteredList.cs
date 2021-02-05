using System.Collections.Generic;

namespace UsedProductExchange.Core.Filter
{
    public class FilteredList<T>
    {
        public Filter FilterUsed { get; set; }
        public int TotalCount { get; set; }

        public int ResultsFound { get; set; }
        public List<T> List { get; set; }
    }
}