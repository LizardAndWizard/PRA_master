using System.ComponentModel.DataAnnotations;

namespace app.DTOs
{
    public class ReservationDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        public string StudentId { get; set; } = null!;


        [Required(ErrorMessage = "The {0} is required.")]
        public int InstructorId { get; set; }


        [Required(ErrorMessage = "The {0} is required.")]
        public int StateId { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "The {0} is required.")]
        public DateTime EndDate { get; set; }
    }
}
