using SportStore.Models.Dtos;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models.Results
{
    public class ProductResult : ProductDTO
    {

        [JsonProperty(Order = -11)]
        public Guid ProductId { get; set; }
        
        [JsonProperty(Order = -7)]
        public bool IsAvalibale { get => Quantity > 0; }

        [JsonProperty(Order = 0)]
        public string CategoryName { get; set; }
    }
}
