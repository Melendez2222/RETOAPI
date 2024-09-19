using AutoMapper;
using RETOAPI.DTOs;
using RETOAPI.Models;

namespace RETOAPI.Mapping
{
    public class ProductProfile:Profile
    {
        public ProductProfile() {
            CreateMap<Product, ProductList>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.CategoryProduct.CatProductName));
            CreateMap<ProductCreate,Product>();
            CreateMap<ProductUpdate, Product>();
        }
    }
}
