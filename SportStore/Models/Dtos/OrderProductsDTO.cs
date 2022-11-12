namespace SportStore.Models.Dtos;

public class OrderProductsDTO
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public int Quantity { get; set; }
}
