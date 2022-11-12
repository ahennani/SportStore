namespace SportStore.Models.Dtos;

public class OrderDTO
{        
    [Required]
    [JsonProperty(Order = 2 )]
    public DateTime OrderDate { get; set; } = DateTime.Now;
    
    [Required]
    [JsonProperty(Order = 3 )]
    public Guid UserId { get; set; }
    
    [Required]
    [JsonProperty(Order = 4 , NullValueHandling = NullValueHandling.Ignore)]
    public IEnumerable<Guid> ProductIDs { get; set; }
}
