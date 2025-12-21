using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cooktel_E_commrece.Repository
{
    public class SubCategroyRepository:ISubCategoryRepository
    {
        private readonly AppDbContext _context;

        public SubCategroyRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Add(Subcategory subcategory)
        {
            _context.subcategories.Add(subcategory);
        }

        public void Delete(Subcategory subcategory)
        {
            _context.subcategories.Remove(subcategory);
        }

        public async Task<IEnumerable<Subcategory>> GetAll(int categoryId)
        {
            return await _context.subcategories.Where(x=>x.CategoryId==categoryId).ToListAsync();
        }

        public async Task<Subcategory>GetById(int id)
        {
            return await _context.subcategories.FindAsync(id);
        }

        public void Update(string SubCategoryName, Subcategory subcategory)
        {
            subcategory.Sub_Name = SubCategoryName;
        }

        public async Task<bool> SaveChanges()
        {
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public Task<bool> SaveAllChanges()
        {
            throw new NotImplementedException();
        }
    }
}
