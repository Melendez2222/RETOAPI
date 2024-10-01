using AutoMapper;
using RETOAPI.DTOs;
using RETOAPI.Models;

namespace RETOAPI.Mapping
{
    public class CartProfile:Profile
    {
        public CartProfile()
        {
            CreateMap<CartItemDetail, CartUser>()
            .ForMember(dest => dest.UserId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.CartDetails, opt => opt.Ignore());

            CreateMap<CartItemDetail, CartDetail>()
                .ForMember(dest => dest.IdCart, opt => opt.Ignore())
                .ForMember(dest => dest.CreateAt, opt => opt.Ignore());
        }
    }
}
