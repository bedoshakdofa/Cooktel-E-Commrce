using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Helper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Cooktel_E_commrece.Interfaces
{
    public interface IProductRepository
    {
        void CreatProduct(Product product);

        void UpdateProduct(JsonPatchDocument<ProductResponse> productDto, Product product, ModelStateDictionary modelState);

        void DeleteProduct(Product product);

        Task<PagedList<ProductResponse>>GetAll(FilterParams userParams);

        Task<Product> GetById(int id);

        Task<ProductWithReviews> GetProductWithReview(int id);

        Task<bool> SaveChanges();

    }
}
