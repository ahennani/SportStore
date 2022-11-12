namespace SportStore.Models.Results;

public class OrderReult : OrderDTO
{
    [JsonProperty(Order = 1)]
    public Guid OrderId { get; set; }

    [JsonProperty(Order = 4, NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<ProductResult> Products { get; set; }
}
