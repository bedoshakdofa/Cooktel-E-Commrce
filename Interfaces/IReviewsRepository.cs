using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;

namespace Cooktel_E_commrece.Interfaces
{
    public interface IReviewsRepository
    {
        void CreateReview(Reviews reviews);

        void DeleteReview(Reviews reviews);

        void UpdateReview(Reviews reviews);

        Task<bool> SaveChanges();

        Task<Reviews> GetReviewsById(int id);
    }
}
