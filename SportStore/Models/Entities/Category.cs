namespace SportStore.Models.Entities;

public class Category
{
    public Guid CategoryId { get; set; }
    public string Name { get; set; }

    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public virtual List<Product> Products { get; set; }
}
