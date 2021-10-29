using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models.Dtos
{
    public class ProductSearchingDTO
    {
        public string SKU { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Name { get; set; }
        public string SearchTerm { get; set; }
    }
}
