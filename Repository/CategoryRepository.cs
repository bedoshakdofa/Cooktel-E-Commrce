using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Interfaces;
using Cooktel_E_commrece.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Cooktel_E_commrece.Repository
{
    public class CategoryRepository:ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Create(string name)
        {
            _context.categories.Add(new Category { Name=name});
        }

        public void Delete(Category category)
        {
            _context.categories.Remove(category);
        }

        public async Task<Category> GetOne(int id)
        {
            return await _context.categories.FindAsync(id);
        }

        public async Task<bool> SaveChanges()
        {
            if (await _context.SaveChangesAsync()>0)
                return true;
            return false;

        }

        public async Task<IEnumerable<Category>> GetAll()
        {
            return await _context.categories.ToListAsync();
        }
    }
}
