using System.ComponentModel.DataAnnotations;

namespace app.DTOs
{
    public class RequestDto
    {
        public int Idrequest { get; set; }

        [Required]
        public string StudentId { get; set; } = null!;

        [Required]
        public int InstructorId { get; set; }

        [Required]
        public int VehicleId { get; set; }

        [Required]
        public int StateId { get; set; }
    }
}
