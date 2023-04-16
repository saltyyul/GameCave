using GameCave.Data.Attributes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCave.Models.ViewModels.GameVMs
{
    public class GameViewModel
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0, 100000)]
        public double Price { get; set; }

        [Required]
        [Range(0, 100000)]
        public int Quantity { get; set; }

        public string? ImageURL { get; set; }

        [NotMapped]
        [DisplayName("Upload file")]
        public IFormFile? ImageFile { get; set; }

        public int CompanyID { get; set; }

        [DisplayName("Genres")]
        [EnsureOneElement(ErrorMessage = "Please select at least one genre!")]
        public List<int>? GenreIds { get; set; }

        public DateTime Created { get; set; }

    }
}
