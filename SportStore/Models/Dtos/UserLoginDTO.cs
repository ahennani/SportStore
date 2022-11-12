namespace SportStore.Models.Dtos;

public class UserLoginDTO
{
    [Required]
    [StringLength(150)]
    public String Username { get; set; }

    [Required]
    [StringLength(20)]
    public String Password { get; set; }
}
