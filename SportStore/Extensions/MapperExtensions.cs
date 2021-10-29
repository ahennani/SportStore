using AutoMapper;
using SportStore.Models.Dtos;
using SportStore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Extensions
{
    public static class MapperExtensions
    {
        public static ProductDTO ConvertToProductDTO(this Product product, IMapper mapper)
        {
            var productDTO = mapper.Map<ProductDTO>(product);
            return productDTO;
        }
    }
}
