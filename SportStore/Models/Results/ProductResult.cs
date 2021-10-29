using SportStore.Models.Dtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models.Results
{
    public class ProductResult : ProductDTO
    {

        //[Required]
        public Guid ProductId { get; set; }

        public bool IsAvalibale { get => Quantity > 0; }
        public string CategoryName { get; set; }
    }
}
