namespace app.DTOs
{
    public class InstructorRegisterDto
    {
        public int Id { get; set; }

        public string FirstName { get; set; } = null!;

        public string Lastname { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int VehicleId { get; set; }
    }
}
