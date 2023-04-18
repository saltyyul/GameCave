using GameCave.Models.ViewModels.ReviewVMs;

namespace GameCave.Services
{
    public interface IReviewService
    {
        public Task<ReviewViewModel> GetReviewByIdAsync(int reviewId);
        public Task<ICollection<ReviewViewModel>> GetAllReviewsAsync();
        public Task<ReviewViewModel> CreateAsync(ReviewViewModel reviewView, string userId);
        public Task<ReviewViewModel> UpdateAsync(ReviewViewModel reviewView, string userId);
        public Task<ReviewViewModel> DeleteAsync(int reviewId);
    }
}
