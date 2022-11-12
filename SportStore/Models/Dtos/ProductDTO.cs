namespace SportStore.Models.Dtos;

public class ProductDTO
{
    [Required, JsonProperty(Order = -10)]
    public string SKU { get; set; }

    [Required, JsonProperty(Order = -9)]
    public string Name { get; set; }

    [Required, JsonProperty(Order = -8)]
    public string Descriptio { get; set; }

    [Required, JsonProperty(Order = -6)]
    public decimal Price { get; set; }

    [Required, Range(minimum: 0, maximum: Int32.MaxValue)]
    public int Quantity { get; set; }

    [Required]

    [JsonProperty(Order = 1)]
    public Guid CategoryId { get; set; }
}
