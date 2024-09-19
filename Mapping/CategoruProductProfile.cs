using AutoMapper;
using RETOAPI.DTOs;
using RETOAPI.Models;

namespace RETOAPI.Mapping
{
    public class CategoruProductProfile:Profile
    {
        public CategoruProductProfile()
        {
            CreateMap<CategoryProduct, CategoryProductList>();
        }
    }
}
