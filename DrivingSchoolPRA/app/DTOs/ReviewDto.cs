using app.Models;
using System.ComponentModel.DataAnnotations;

namespace app.DTOs
{
    public class ReviewDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        public string StudentOIB { get; set; } = null!;

        [Required(ErrorMessage = "The {0} is required.")]
        public int InstructorId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        public int Grade { get; set; }

        public string? Comment { get; set; }
    }
}
