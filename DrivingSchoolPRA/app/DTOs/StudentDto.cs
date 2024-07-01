using app.Models;

namespace app.DTOs
{
    public class StudentDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public int? HoursDriven { get; set; }

        public string Oib { get; set; } = null!;

        public int? InstructorId { get; set; }

        public string Password { get; set; } = null!;

        public int? VehicleId { get; set; }

        public VehicleDto? Vehicle { get; set; }
    }
}
