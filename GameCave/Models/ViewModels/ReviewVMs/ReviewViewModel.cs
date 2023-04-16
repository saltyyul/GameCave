using GameCave.Data;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace GameCave.Models.ViewModels.ReviewVMs
{
    public class ReviewViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Description is too short!")]
        [MaxLength(int.MaxValue, ErrorMessage = "Description is too long!")]
        public string Description { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Title is too short!")]
        [MaxLength(500, ErrorMessage = "Title is too long!")]
        public string Title { get; set; }

        public int GameID { get; set; }
        public virtual Game? Game { get; set; }

        public DateTime Created { get; set; }
        public string? CreatedById { get; set; }
        public IdentityUser? CreatedBy { get; set; }

        public DateTime? Modified { get; set; }
        public string? ModifiedById { get; set; }
        public IdentityUser? ModifiedBy { get; set; }
    }
}
