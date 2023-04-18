using GameCave.Data;
using GameCave.Models.ViewModels.ReviewVMs;
using GameCave.Repositories;
using Microsoft.AspNetCore.Identity;

namespace GameCave.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<Game> _gameRepository;
        private readonly IRepository<Review> _reviewRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public ReviewService(IRepository<Game> gameRepository, IRepository<Review> reviewRepository, UserManager<IdentityUser> userManager)
        {
            _gameRepository = gameRepository;
            _reviewRepository = reviewRepository;
            _userManager = userManager;
        }
        public async Task<ReviewViewModel> CreateAsync(ReviewViewModel reviewView, string userId)
        {
            /*Id 
            Description 
            Title 
            int GameID 
            Game? Game 
            DateTime? Created 
            int? CreatedById 
            IdentityUser? CreatedBy 
            DateTime? Modified 
            int? ModifiedById 
            IdentityUser? ModifiedBy */

            Review review = new Review();
            review.Title = reviewView.Title;
            review.Description = reviewView.Description;
            review.GameID = reviewView.GameID;
            review.Game = await _gameRepository.GetByIdAsync(review.GameID);
            review.Created = DateTime.Now;
            review.CreatedById = userId;
            review.CreatedBy = await _userManager.FindByIdAsync(review.CreatedById);
            review.Modified = DateTime.Now;
            review.ModifiedById = userId;
            review.ModifiedBy = await _userManager.FindByIdAsync(review.ModifiedById);

            await _reviewRepository.CreateAsync(review);

            return reviewView;
        }

        public async Task<ReviewViewModel> DeleteAsync(int reviewId)
        {
            Review review = await _reviewRepository.GetByIdAsync(reviewId);
            if (review != null)
            {
                ReviewViewModel reviewView = new ReviewViewModel();
                reviewView.Id = reviewId;
                reviewView.Title = review.Title;
                reviewView.Description = review.Description;
                reviewView.Created = review.Created;
                reviewView.CreatedById = review.CreatedById;
                reviewView.CreatedBy = review.CreatedBy == null ? (await _userManager.FindByIdAsync(review.CreatedById)) : review.CreatedBy;
                reviewView.Modified = review.Modified;
                reviewView.ModifiedById = review.ModifiedById;
                reviewView.ModifiedBy = review.ModifiedBy == null ? (await _userManager.FindByIdAsync(review.ModifiedById)) : review.ModifiedBy;
                reviewView.GameID = review.GameID;
                reviewView.Game = await _gameRepository.GetByIdAsync(reviewView.GameID);

                await _reviewRepository.DeleteAsync(review.Id);
                return reviewView;
            }

            return null;
        }

        public async Task<ICollection<ReviewViewModel>> GetAllReviewsAsync()
        {
            var reviews = await _reviewRepository.GetAllAsync();
            ICollection<ReviewViewModel> result = new List<ReviewViewModel>();

            if (reviews.Count > 0)
            {
                foreach (var review in reviews)
                {
                    ReviewViewModel reviewView = new ReviewViewModel();
                    reviewView.Id = review.Id;
                    reviewView.Title = review.Title;
                    reviewView.Description = review.Description;
                    reviewView.Created = review.Created;
                    reviewView.CreatedById = review.CreatedById;
                    reviewView.CreatedBy = review.CreatedBy == null ? (await _userManager.FindByIdAsync(review.CreatedById)) : review.CreatedBy;
                    reviewView.Modified = review.Modified;
                    reviewView.ModifiedById = review.ModifiedById;
                    reviewView.ModifiedBy = review.ModifiedBy == null ? (await _userManager.FindByIdAsync(review.ModifiedById)) : review.ModifiedBy;
                    reviewView.GameID = review.GameID;
                    reviewView.Game = await _gameRepository.GetByIdAsync(reviewView.GameID);
                    result.Add(reviewView);
                }

                return result;
            }
            return null;
        }

        public async Task<ReviewViewModel> GetReviewByIdAsync(int reviewId)
        {
            Review review = await _reviewRepository.GetByIdAsync(reviewId);

            if (review == null)
            {
                return null;
            }

            ReviewViewModel reviewView = new ReviewViewModel();
            reviewView.Id = review.Id;
            reviewView.Title = review.Title;
            reviewView.Description = review.Description;
            reviewView.Created = review.Created;
            reviewView.CreatedById = review.CreatedById;
            reviewView.CreatedBy = review.CreatedBy == null ? (await _userManager.FindByIdAsync(review.CreatedById)) : review.CreatedBy;
            reviewView.Modified = review.Modified;
            reviewView.ModifiedById = review.ModifiedById;
            reviewView.ModifiedBy = review.ModifiedBy == null ? (await _userManager.FindByIdAsync(review.ModifiedById)) : review.ModifiedBy;
            reviewView.GameID = review.GameID;
            reviewView.Game = await _gameRepository.GetByIdAsync(reviewView.GameID);

            return reviewView;
        }

        //public async Task<Review> MapFromViewModelToEntity(ReviewViewModel viewModel)
        //{
        //    Review review = new Review();
        //    review.Id = viewModel.Id;
        //    review.Title = viewModel.Title;
        //    review.Description = viewModel.Description;
        //    review.
        //    return default;
        //}

        public async Task<ReviewViewModel> UpdateAsync(ReviewViewModel reviewView, string userId)
        {
            Review oldReview = await _reviewRepository.GetByIdAsync(reviewView.Id);
            Game game = await _gameRepository.GetByIdAsync(oldReview.GameID);

            Review review = new Review()
            {
                Id = oldReview.Id,
                Title = reviewView.Title,
                Description = reviewView.Description,
                GameID = oldReview.GameID,
                Game = game,
                Created = oldReview.Created,
                CreatedById = oldReview.CreatedById,
                CreatedBy = oldReview.CreatedBy,
                Modified = DateTime.Now,
                ModifiedById = userId,
                ModifiedBy = await _userManager.FindByIdAsync(userId)
            };

            await _reviewRepository.UpdateAsync(review);
            return reviewView;
        }
    }
}
