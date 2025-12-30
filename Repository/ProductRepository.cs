using AutoMapper;
using AutoMapper.QueryableExtensions;
using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Helper;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Cooktel_E_commrece.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ProductRepository(AppDbContext context
            ,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
           
        }

        public void CreatProduct(Product product)
        {
            _context.products.Add(product);
        }

        public void DeleteProduct(Product product)
        {
            _context.products.Remove(product);
        }

        public async Task<PagedList<ProductResponse>> GetAll(FilterParams userParams)
        {
           var query=_context.products
                .Include(x=>x.subcategory)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrEmpty(userParams.size))
            {
                query=query.Where(x=>x.size==userParams.size);
            }

            if (!string.IsNullOrEmpty(userParams.color))
            {
                query=query.Where(x=>x.color==userParams.color);
            }

            if (!string.IsNullOrEmpty(userParams.OrderByPrice))
            {
                query = userParams.OrderByPrice switch
                {
                    "desc" => query.OrderByDescending(p => p.Price),
                    _ => query.OrderBy(x => x.Price)

                };
             }

            if (userParams.SubCategoryId > 0)
            {
                query = query.Where(x=>x.SubCategoryID==userParams.SubCategoryId);
            }

            if (userParams.categoryId > 0)
            {
                query=query.Where(x=>x.subcategory.CategoryId==userParams.categoryId);
            }

            return await PagedList<ProductResponse>.CreateAsync(query.ProjectTo<ProductResponse>(_mapper.ConfigurationProvider),
                userParams.PageNumber, userParams.PageSize);
        }

        public async Task<Product> GetById(int id)
        {
            return await _context.products.FindAsync(id);
        }

        public async Task<ProductWithReviews> GetProductWithReview(int id)
        {
            var query=_context.products.AsQueryable();

            var result = await query.AsNoTracking()
                .Include(x => x.Reviews)
                .ProjectTo<ProductWithReviews>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(x => x.id == id);

            return result;
        }

        public void UpdateProduct(JsonPatchDocument<ProductResponse> productDto,Product product, ModelStateDictionary modelState)
        {
            var productInDto = _mapper.Map<ProductResponse>(product);

            productDto.ApplyTo(productInDto, modelState);

            _mapper.Map(productInDto, product);
        }

        public void ReduceProductStock(Product prod)
        {
            prod.ProductStock--;
        }

        public async Task<bool> SaveChanges()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
