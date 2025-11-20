using AutoMapper;
using Cooktel_E_commrece.Data;
using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;

namespace Cooktel_E_commrece.Repository
{
    public class ReviewRepository:IReviewsRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ReviewRepository(AppDbContext context,IMapper mapper)
        {
            _mapper=mapper;
            _context = context;
        }

        public void CreateReview(Reviews reviews)
        {

            _context.reviews.Add(reviews);
        }

        public void DeleteReview(Reviews reviews)
        {
            _context.reviews.Remove(reviews);
        }

        public async Task<Reviews> GetReviewsById(int id)
        {
            return await _context.reviews.FindAsync(id);
        }

        public async Task<bool> SaveChanges()
        {
            if (await _context.SaveChangesAsync()>0)
                return true;
            return false;
        }

        public void UpdateReview(Reviews reviews)
        {
            _context.reviews.Update(reviews);
        }
    }
}
