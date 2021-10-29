using Newtonsoft.Json;

namespace SportStore.Models
{
    public class PagingDetails
    {
        public int TotalRows { get; set; }
        public int TotalPages { get; set; }
        public int CurPage { get; set; }
        public bool HasNextPage { get; set; }
        public bool HasPrevPage { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string NextPageURL { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrevPageURL { get; set; }
    }
}
