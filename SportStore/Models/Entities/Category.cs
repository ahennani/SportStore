using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace SportStore.Models.Entities
{
    public class Category
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public virtual List<Product> Products { get; set; }
    }
}
