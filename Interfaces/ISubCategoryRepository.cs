using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;

namespace Cooktel_E_commrece.Interfaces
{
    public interface ISubCategoryRepository
    {
        Task<IEnumerable<SubCategoryDto>> GetAll(int categoryId);

        void Add(Subcategory subcategory);

        void Update(string SubCategoryName,Subcategory subcategory);

        void Delete(Subcategory subcategory);

        Task<Subcategory> GetById(int id);

        Task<bool> SaveAllChanges();

    }
}
