using SportStore.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SportStore.Models.Results
{
    public class PagingResult<T> where T : class
    {
        public PagingDetails Paging { get; set; }
        public IQueryable<T> Data { get; set; }

        public PagingResult(IEnumerable<T> query, PagingDTO clientPaging)
        {
            Paging = new();

            Paging.TotalRows = query.Count();
            Paging.TotalPages = Convert.ToInt32(Math.Ceiling((double)Paging.TotalRows / clientPaging.Size));
            Paging.CurPage = clientPaging.Page;
            Paging.HasNextPage = Paging.CurPage < Paging.TotalPages;
            Paging.HasPrevPage = Paging.CurPage > 1;

            var paginatingOrder = (clientPaging.Size * (clientPaging.Page - 1));
            var skip = (paginatingOrder < 0) ? 0 : paginatingOrder;
            var take = clientPaging.Size;

            Data = query.Skip(skip).Take(take).AsQueryable();
        }
    }
}
