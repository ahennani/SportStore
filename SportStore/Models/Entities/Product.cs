namespace SportStore.Models.Entities;

public class Product
{
    [Key]
    public Guid ProductId { get; set; }
    public string SKU { get; set; }
    public string Name { get; set; }
    public string Descriptio { get; set; }
    public decimal Price { get; set; }
    public bool IsAvalibale { get => Quantity > 0; }

    [Range(minimum: 0, maximum: Int32.MaxValue)]
    public int Quantity { get; set; }

    public Guid? OrderId { get; set; }
    public Guid CategoryId { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public virtual Order Order { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public virtual Category Category { get; set; }

}
