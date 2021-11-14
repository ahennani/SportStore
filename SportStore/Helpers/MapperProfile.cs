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
            // Users
            CreateMap<User, UserResult>();
            CreateMap<UserAddDTO, User>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => Guid.NewGuid()));

            // Products
            CreateMap<ProductDTO, Product>();
            CreateMap<Product, ProductResult>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

            // Categories
            CreateMap<Category, CategoryResult>();
            CreateMap<CategoryDTO, Category>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => Guid.NewGuid()));

            // Orders
            CreateMap<Order, OrderReult>()
                .ForMember(dest => dest.Products, opt => opt.Condition(p => (p.Products.Count != 0) || p.Products is null));
                //.ForMember(dest => dest.Products, opt => opt.MapFrom(src => ));
            CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => Guid.NewGuid()));

        }
    }
}
