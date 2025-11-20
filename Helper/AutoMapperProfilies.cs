using AutoMapper;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
namespace Cooktel_E_commrece.Helper
{
    public class AutoMapperProfilies:Profile
    {
        public AutoMapperProfilies()
        {
            //mapping for user
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            //Mapping for product
            CreateMap<ProductRequest, Product>()
                .ForMember(src=>src.Image,
                opt=>opt.Ignore());
            CreateMap<Product, ProductRequest>();
            CreateMap<Product, ProductResponse>();
            CreateMap<ProductResponse, Product>()
                .ForMember(src=>src.CategoryID,opt=>opt.Ignore());
            CreateMap<Product, ProductWithReviews>();

            //Category Mapping
            CreateMap<Category, CategoryDto>();

            //ReviewMapping
            CreateMap<Reviews,ReviewDto>();

            //CartItemMapping

            CreateMap<CartItems,CartItemsResponse>();
        }
    }
}
