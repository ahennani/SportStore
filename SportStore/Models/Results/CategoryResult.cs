using SportStore.Models.Dtos;
using System;

namespace SportStore.Models.Results
{
    public class CategoryResult : CategoryDTO
    {
        public Guid CategoryId { get; set; }
    }
}
