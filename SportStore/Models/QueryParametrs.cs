using System;

namespace SportStore.Models
{
    public class QueryParametrs
    {
        // Paginating
        private const int _maxSize = 100;
        private int _size = 10;
        public int Size
        {
            get => _size; 
            set => _size = Math.Min(_maxSize, value);
        }

        public int Page { get; set; }

        // Filtring - Searching
        public string SKU { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Name { get; set; }
        public string SearchTerm { get; set; }

        // Sorting
        public string SortBy { get; set; } = "Id";

        private string _sortOrder = "asc";
        public string SortOrder
        {
            get { return _sortOrder; }
            set { if (value == "asc" || value == "desc") _sortOrder = value; }
        }
    }
}
