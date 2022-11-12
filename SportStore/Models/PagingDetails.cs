namespace SportStore.Models;

public class PagingDetails
{
    public int TotalRows { get; set; }
    public int TotalPages { get; set; }
    public int CurPage { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPrevPage { get; set; }

    [JsonIgnore]
    public string NextPageURL { get; set; }

    [JsonIgnore]
    public string PrevPageURL { get; set; }
}
