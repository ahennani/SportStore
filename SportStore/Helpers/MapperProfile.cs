using AutoMapper;
using SportStore.Models.Entities;
using SportStore.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SportStore.Models.Results;

namespace SportStore.Helpers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserAddDTO, User>().ForMember(d => d.UserId, opt => opt.MapFrom(s => Guid.NewGuid()));

            CreateMap<ProductDTO, Product>();
            CreateMap<Product, ProductResult>().ForMember(pd => pd.CategoryName, opt => opt.MapFrom(p => p.Category.Name));

        } 
    }
}
