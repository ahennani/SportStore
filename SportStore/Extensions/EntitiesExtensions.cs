using SportStore.Models.Dtos;
using SportStore.Models.Entities;

namespace SportStore.Extensions
{
    public static class EntitiesExtensions
    {
        public static Product Update(this Product product, ProductDTO productDTO)
        {
            product.Name = productDTO.Name;
            product.SKU = productDTO.SKU;
            product.Price = productDTO.Price;
            product.Quantity = productDTO.Quantity;
            product.Descriptio = productDTO.Descriptio;
            product.CategoryId = productDTO.CategoryId;

            return product;
        }

        public static Category Update(this Category category, CategoryDTO categoryDTO)
        {
            category.Name = categoryDTO.Name;

            return category;
        }
    }
}
