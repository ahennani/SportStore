using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SportStore.Models.Entities
{
    public class Order
    {
        public Order()
        {
            Products = new();
        }

        [Key]
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        public Guid UserId { get; set; }

        public virtual User User { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<Product> Products { get; set; }
    }
}
