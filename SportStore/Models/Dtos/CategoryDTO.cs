using System.ComponentModel.DataAnnotations;

namespace SportStore.Models.Dtos
{
    public class CategoryDTO
    {
        [Required, MinLength(4), MaxLength(20)]
        public string Name { get; set; }
    }
}
