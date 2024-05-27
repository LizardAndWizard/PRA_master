using app.Models;

namespace app.DTOs
{
    public class InstructorDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public float Rating { get; set; }

        public int VehicleId { get; set; }

        public virtual Vehicle Vehicle { get; set; } = null!;
    }
}
