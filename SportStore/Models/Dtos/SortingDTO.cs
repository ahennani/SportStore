using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models.Dtos
{
    public class SortingDTO
    {
        private string _sortOrder = "asc";

        public string SortBy { get; set; }
        public string SortOrder
        {
            get => _sortOrder; 
            set { if (value == "asc" || value == "desc") { _sortOrder = value; } }
        }
    }
}
