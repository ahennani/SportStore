namespace SportStore.Models.Dtos;

public class PagingDTO
{
    private const int _maxSize = 50;
    private int _size = 10;

    public int Size { get => _size; set => _size = Math.Min(_maxSize, value); }
    public int Page { get; set; } = 1;
}
