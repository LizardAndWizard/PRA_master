using System.ComponentModel.DataAnnotations;

namespace app.DTOs
{
    public class RequestDto
    {
        public int Idrequest { get; set; }

        public string StudentId { get; set; } = null!;

        public string? StudentName { get; set; }

        public int InstructorId { get; set; }

        public int VehicleId { get; set; }

        public int StateId { get; set; }
    }
}
