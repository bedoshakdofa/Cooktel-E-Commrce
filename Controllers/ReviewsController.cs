using Cooktel_E_commrece.Data.Models;
using Cooktel_E_commrece.Dtos;
using Cooktel_E_commrece.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Cooktel_E_commrece.Controllers
{
    [ApiController]
    [Route("/api/[Controller]")]
    [Authorize]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewsRepository _reviewsRepository;
        private readonly ICachingService _cachingService;

        public ReviewsController(IReviewsRepository reviewsRepository, ICachingService cachingService)
        {
            _reviewsRepository = reviewsRepository;
            _cachingService = cachingService;
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddReview(ReviewRequest request)
        {
            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var newReview = new Reviews
            {
                Comment = request.Comment,
                User_ID = Guid.Parse(userID),
                Product_ID = request.Product_ID,
            };
            _reviewsRepository.CreateReview(newReview);

            if (await _reviewsRepository.SaveChanges())
            {
                await _cachingService.RemoveCache<ProductResponse>($"product:{request.Product_ID}");
                return Ok("Review Added");
            }

            return BadRequest("Can't added review");
        }

        [HttpDelete("{id}")]

        public async Task<ActionResult> DeleteReview([FromRoute] int id)
        {
            var review = await _reviewsRepository.GetReviewsById(id);

            if (review == null)
                return NotFound("there is no review with this id");

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (review.User_ID != Guid.Parse(userID))
                return Unauthorized("this user can't delete this review");

            _reviewsRepository.DeleteReview(review);

            if (await _reviewsRepository.SaveChanges())
            {
                await _cachingService.RemoveCache<ProductWithReviews>($"product:{review.Product_ID}");
                return Ok("Review Deleted");
            }

            return BadRequest("Can't Deleted review");
        }

        [HttpPut("{id}")]

        public async Task<ActionResult> UpdateReview([FromBody] string comment, [FromRoute] int id)
        {
            var review = await _reviewsRepository.GetReviewsById(id);

            if (review == null)
                return NotFound("there is no review with this id");

            var userID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (review.User_ID != Guid.Parse(userID))
                return Unauthorized("this user can't delete this review");

            review.Comment = comment;

            if (await _reviewsRepository.SaveChanges())
            {
                await _cachingService.RemoveCache<ProductWithReviews>($"product:{review.Product_ID}");
                return Ok("Review Updated");
            }
            return BadRequest("Can't Update review");
        }
    }
}
