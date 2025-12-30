using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Cooktel_E_commrece.Repository
{
    public class SubCategroyRepository:ISubCategoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public SubCategroyRepository(AppDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public void Add(Subcategory subcategory)
        {
            _context.subcategories.Add(subcategory);
        }

        public void Delete(Subcategory subcategory)
        {
            _context.subcategories.Remove(subcategory);
        }

        public async Task<IEnumerable<SubCategoryDto>> GetAll(int categoryId)
        {
            return await _context.subcategories
                .Where(x=>x.CategoryId==categoryId)
                .ProjectTo<SubCategoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<Subcategory>GetById(int id)
        {
            return await _context.subcategories.FindAsync(id);
        }

        public void Update(string SubCategoryName, Subcategory subcategory)
        {
            subcategory.Sub_Name = SubCategoryName;
        }

        public async Task<bool> SaveAllChanges()
        {
            if (await _context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
