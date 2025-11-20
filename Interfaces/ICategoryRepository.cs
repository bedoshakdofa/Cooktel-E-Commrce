using Cooktel_E_commrece.Data.Models;

namespace Cooktel_E_commrece.Interfaces
{
    public interface ICategoryRepository
    {
        void Create(string name);

        void Delete(Category category);

        Task<Category> GetOne(int id);

        Task<IEnumerable<Category>> GetAll();

        Task<bool> SaveChanges();
    }
}
