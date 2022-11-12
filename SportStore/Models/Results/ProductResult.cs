namespace SportStore.Models.Results;

public class ProductResult : ProductDTO
{

    [JsonProperty(Order = -11)]
    public Guid ProductId { get; set; }
    
    [JsonProperty(Order = -7)]
    public bool IsAvalibale { get => Quantity > 0; }

    [JsonProperty(Order = 0)]
    public string CategoryName { get; set; }
}
