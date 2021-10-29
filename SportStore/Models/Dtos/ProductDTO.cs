using SportStore.Models.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models.Dtos
{
    public class ProductDTO
    {
        [Required]
        public string SKU { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Descriptio { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required, Range(minimum: 0, maximum: Int32.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public Guid CategoryId { get; set; }
    }
}
