using AutoMapper;
using RETOAPI.DTOs;
using RETOAPI.Models;

namespace RETOAPI.Mapping
{
    public class ProductProfile:Profile
    {
        public ProductProfile() {
            CreateMap<Users, UserList>()
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.UserRols.FirstOrDefault().Rols.RolName));
            CreateMap<ProductCreate,Product>();
            CreateMap<ProductUpdate, Product>();
        }
    }
}
